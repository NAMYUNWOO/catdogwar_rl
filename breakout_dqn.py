import pylab
import random
import copy
import numpy as np
import tensorflow as tf
from collections import deque
from keras.models import Sequential
from keras.optimizers import Adam,RMSprop,Adagrad
from keras.layers import Dense, Flatten
from keras.layers.convolutional import Conv2D
from keras import backend as K
import time
import zmq
import math
context = zmq.Context()
socket = context.socket(zmq.REQ)
socket.connect("tcp://localhost:12346")
TIMEOUT = 10000

state_hist = []
EPISODES = 5000000


class DQNAgent:
    def __init__(self, state_size,action_size):
        self.load_model = False
        self.state_size =  state_size
        self.action_size = action_size
        self.epsilon = 1.0
        self.epsilon_start, self.epsilon_end = 1.0, 0.001
        self.exploration_steps = 500.
        self.epsilon_decay_step = (self.epsilon_start - self.epsilon_end) / self.exploration_steps
        self.batch_size = 64
        self.train_start = 5000
        self.update_target_rate = 1000
        self.discount_factor = 0.90
        self.learning_rate = 0.001
        self.memory = deque(maxlen=10000)
        self.model = self.build_model()
        self.target_model = self.build_model()
        if self.load_model:
            self.model.load_weights("./save_model/breakout_dqn6_.h5")
        self.update_target_model()
        self.sess = tf.InteractiveSession()
        K.set_session(self.sess)

        self.avg_q_max, self.avg_loss = 0, 0
        self.summary_placeholders, self.update_ops, self.summary_op = self.setup_summary()
        self.summary_writer = tf.summary.FileWriter("./summary/breakout_dqn", self.sess.graph)
        self.sess.run(tf.global_variables_initializer())



    def build_model(self):
        model = Sequential()
        model.add(Dense(16,input_dim=self.state_size,activation='relu',kernel_initializer="he_uniform"))
        model.add(Dense(16,activation='relu',kernel_initializer="he_uniform"))
        model.add(Dense(self.action_size,activation='linear',kernel_initializer="he_uniform"))
        model.summary()
        model.compile(loss='mse', optimizer=Adam(lr=self.learning_rate))
        return model

    def update_target_model(self):
        self.target_model.set_weights(self.model.get_weights())

    def get_action(self, state):
        if np.random.rand() <= self.epsilon:
            return random.randrange(self.action_size)
        else:
            q_value = self.model.predict(state)
            self.avg_q_max += np.amax(q_value)
            return np.argmax(q_value[0])

    def append_memory(self, state, action, reward, next_state, dead):
        self.memory.append((state, action, reward, next_state, dead))

    def train_replay(self):
        if len(self.memory) < self.train_start:
            return
        if self.epsilon > self.epsilon_end:
            self.epsilon -= self.epsilon_decay_step

        mini_batch = random.sample(self.memory, self.batch_size)

        state = np.zeros((self.batch_size, self.state_size))
        next_state = np.zeros((self.batch_size, self.state_size))
        action, reward, dead = [], [], []

        for i in range(self.batch_size):
            state[i] = mini_batch[i][0]
            next_state[i] = mini_batch[i][3]
            action.append(mini_batch[i][1])
            reward.append(mini_batch[i][2])
            dead.append(mini_batch[i][4])

        target = self.model.predict(state)
        target_value = self.target_model.predict(next_state)

        for i in range(self.batch_size):
            if dead[i]:
                target[i][action[i]] = reward[i]
            else:
                target[i][action[i]] = reward[i] + self.discount_factor * \
                                        np.amax(target_value[i])
        self.model.fit(state, target, batch_size=self.batch_size, epochs=1, verbose=0)

    def save_model(self, name):
        self.model.save_weights(name)

    def setup_summary(self):
        episode_total_reward = tf.Variable(0.)
        episode_avg_max_q = tf.Variable(0.)
        episode_duration = tf.Variable(0.)

        tf.summary.scalar('Total Reward/Episode', episode_total_reward)
        tf.summary.scalar('Average Max Q/Episode', episode_avg_max_q)
        tf.summary.scalar('Duration/Episode', episode_duration)

        summary_vars = [episode_total_reward, episode_avg_max_q,episode_duration]
        summary_placeholders = [tf.placeholder(tf.float32) for _ in
                                range(len(summary_vars))]
        update_ops = [summary_vars[i].assign(summary_placeholders[i]) for i in
                      range(len(summary_vars))]
        summary_op = tf.summary.merge_all()
        return summary_placeholders, update_ops, summary_op

def getPlayerAction(player_y,coin_y,skul_y,player_skul_dist_x,player_coin_dist,player_skul_dist):
	playerAction = ""
	if player_skul_dist_x < 3.0:
		if player_skul_dist < 0.5:
			playerAction = "2"
		elif player_y > skul_y:
			playerAction = "1"
		else:
			playerAction = "0"
	elif min(player_coin_dist,player_skul_dist) < 0.5:
		if player_coin_dist < player_skul_dist:
			playerAction = "3"
		else:
			playerAction = "2"
	else:
		if player_coin_dist < player_skul_dist:
			if player_y > coin_y :
				playerAction = "1"
			else:
				playerAction = "0"
		else:
			if player_y > skul_y:
				playerAction = "1"
			else:
				playerAction = "0"
	return playerAction
	
actions = ["0", "1", "2","3","4"]  # up, down, push ,pull , stay
						   #    0       1    2     3         4    5    6     7       8        9     10    
def getAction(cur_state):  # [cat_y,  c_x, c_y, sc_c_x, sc_c_y, s_x, s_y, sc_s_x, sc_s_y,  dog_y, isEnd]
	catAction = ""
	dogAction = ""
	cat_y = cur_state[0]
	cat_x = -8.0
	dog_x = 8.0
	dog_y = cur_state[-1]
	coin_y = cur_state[2]
	skul_x = cur_state[5]
	skul_y = cur_state[6] 
	
	cat_coin_dist = abs(cat_y - coin_y)
	cat_skul_dist = abs(cat_y - skul_y)
	dog_coin_dist = abs(dog_y - coin_y)
	dog_skul_dist = abs(dog_y - skul_y)
	cat_skul_dist_x = abs(cat_x - skul_x)
	dog_skul_dist_x = abs(dog_x - skul_x)
	catAction = getPlayerAction(cat_y,coin_y,skul_y,cat_skul_dist_x,cat_coin_dist,cat_skul_dist)
	dogAction = getPlayerAction(dog_y,coin_y,skul_y,dog_skul_dist_x,dog_coin_dist,dog_skul_dist)
	time.sleep(0.0001)  # calculate time 
	return [int(catAction),int(dogAction)]

						           #    0       1    2     3        4    5    6     7       8        9  
def getStat(state_raw,state_size): # [cat_y,  c_x, c_y, sc_c_x, sc_c_y, s_x, s_y, sc_s_x, sc_s_y,  dog_y]
    cat_x = -8.0
    cat_y = state_raw[0]
    dog_x = 8.0
    dog_y = state_raw[-1]

    coin_x = state_raw[1]
    coin_y = state_raw[2]
    skul_x = state_raw[5]
    skul_y = state_raw[6] 

    coin_x_s = state_raw[3]
    coin_y_s = state_raw[4]
    skul_x_s = state_raw[7]
    skul_y_s = state_raw[8] 

    cat_coin_dist_x = abs(cat_x - coin_x)
    cat_coin_dist_y = (cat_y - coin_y)
    cat_skul_dist_x = abs(cat_x - skul_x)
    cat_skul_dist_y = (cat_y - skul_y)
    cat_skul_scalar_x = -1.0*skul_x_s
    cat_skul_scalar_y = skul_y_s
    cat_coin_scalar_x = -1.0*coin_x_s
    cat_coin_scalar_y = coin_y_s

    dog_coin_dist_x = abs(dog_x - coin_x)
    dog_coin_dist_y = (dog_y - coin_y)
    dog_skul_dist_x = abs(dog_x - skul_x)
    dog_skul_dist_y = (dog_y - skul_y)
    dog_skul_scalar_x = skul_x_s
    dog_skul_scalar_y = skul_y_s
    dog_coin_scalar_x = coin_x_s
    dog_coin_scalar_y = coin_y_s
    result = [[cat_coin_dist_x,cat_coin_dist_y,cat_skul_dist_x,cat_skul_dist_y,cat_skul_scalar_x,cat_skul_scalar_y,cat_coin_scalar_x,cat_coin_scalar_y],
             [dog_coin_dist_x,dog_coin_dist_y,dog_skul_dist_x,dog_skul_dist_y,dog_skul_scalar_x,dog_skul_scalar_y,dog_coin_scalar_x,dog_coin_scalar_y]]
    result[0] = np.reshape(result[0],[1,state_size])
    result[1] = np.reshape(result[1],[1,state_size])
    return result


if __name__ == "__main__":
    action = "0 1"
    cat_action_prev = 0
    dog_action_prev = 1
    state_size = 8
    action_size = 4
    frameNum_prev = 0
    agent = DQNAgent(state_size=state_size,action_size=action_size)
    scores, episodes, avg_qvals,global_step = [], [],[] ,0
    time_cur = 0
    time_pre = 0
    for e in range(EPISODES):
        done = False
        start_frameNum = 0
        step,score = 0,0
        isStart = False
        cat_state_prev = np.reshape([0 for _ in range(state_size)],[1,state_size])
        dog_state_prev = np.reshape([0 for _ in range(state_size)],[1,state_size])
        while not done:
            #skipframe()
            #time.sleep(0.0001)	
            socket.send_string(action)
            poller = zmq.Poller()
            poller.register(socket, zmq.POLLIN)
            evt = dict(poller.poll(TIMEOUT))
            if evt:
                if evt.get(socket) == zmq.POLLIN:
                    response = socket.recv(zmq.NOBLOCK).decode("utf-8")
                    if response == "res_skip":
                        continue
                    response = np.array(list(map(lambda x:float(x),response.split(" "))))
                    frameNum = int(response[0])
                    if not isStart:
                        start_frameNum = frameNum-1
                        frameNum_prev = frameNum-1
                        isStart = True

                    if frameNum_prev == frameNum:
                        frameNum_prev = frameNum
                        continue

                    global_step += 1
                    step += 1
                    cat_reward = response[1]
                    dog_reward = response[2]
                    if abs(cat_reward) + abs(dog_reward) < 1.0:
                        r = cat_reward + dog_reward
                        score += r
                    state_raw = response[3:-1]
                    time_cur = int(response[-1])
                    if time_cur > time_pre:
                        done = time_cur % 30 == 0
                    time_pre = time_cur
                    cat_state,dog_state = getStat(state_raw,state_size)
                    if frameNum - start_frameNum < 3:
                        cat_action,dog_action = getAction(state_raw)
                    else:
                        cat_action = agent.get_action(cat_state)
                        dog_action = agent.get_action(dog_state)
                        agent.append_memory(cat_state_prev,cat_action_prev,cat_reward,cat_state,False)
                        agent.append_memory(dog_state_prev,dog_action_prev,dog_reward,dog_state,False)
                        agent.train_replay()

                    if global_step % agent.update_target_rate == 0:
                        agent.update_target_model()

                    action = str(cat_action)+" "+str(dog_action)
                    #print(list(map(lambda x : round(x,2),response))+[action])
                    frameNum_prev = frameNum
                    cat_state_prev = cat_state
                    dog_state_prev = dog_state
                    cat_action_prev = cat_action
                    dog_action_prev = dog_action
                    
                    if done:
                        avg_qval = agent.avg_q_max / float(step)
                        if global_step > agent.train_start:
                            stats = [score,avg_qval, step]
                            for i in range(len(stats)):
                                agent.sess.run(agent.update_ops[i], feed_dict={
                                    agent.summary_placeholders[i]: float(stats[i])
                                })
                            summary_str = agent.sess.run(agent.summary_op)
                            agent.summary_writer.add_summary(summary_str, e + 1)
                        scores.append(score)
                        avg_qvals.append(avg_qval)
                        episodes.append(e)
                        print("episode:", e, "  score:", score, "  memory length:",len(agent.memory), "  epsilon:", round(agent.epsilon,4),
                            "  global_step:", global_step, "  average_q:",avg_qval)
                        agent.avg_q_max, agent.avg_loss = 0, 0

                    continue
            print("no evt")
            time.sleep(0.1)
            socket.close()
            socket = context.socket(zmq.REQ)
            socket.connect("tcp://localhost:12346")

        if e % 10 == 0:
            try:
                agent.model.save_weights("./save_model/breakout_dqn_.h5")
                pylab.plot(episodes,scores,'b')
                pylab.savefig("./save_graph/breakout_dqn_score.png")
                #pylab.plot(episodes,avg_qvals,'b')
                #pylab.savefig("./save_graph/breakout_dqn_qval.png")
            except:
                pass
        if len(episodes) % 100 == 0:
            try:
                np.savetxt("./save_graph/scores.csv",scores)
                np.savetxt("./save_graph/avg_qvals.csv",avg_qvals)
            except:
                pass

