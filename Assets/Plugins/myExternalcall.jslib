mergeInto(LibraryManager.library, {

  AddCurScore: function(catScore,dogScore){
    document.getElementById('catScoreCur').innerHTML =parseInt(catScore);
    document.getElementById('dogScoreCur').innerHTML =parseInt(dogScore);
  },
  
  AddCurScore2: function(AvengersScore,ThanosScore){
    document.getElementById('AvengersScoreCur').innerHTML =parseInt(AvengersScore);
    document.getElementById('ThanosScoreCur').innerHTML =parseInt(ThanosScore);
  },
  AddRecord: function(win,tie,lose){
    var curWin = document.getElementById('win').innerHTML;
    var curTie = document.getElementById('tie').innerHTML;
    var curLose = document.getElementById('lose').innerHTML;
    document.getElementById('win').innerHTML =parseInt(win)+ parseInt(curWin);
    document.getElementById('tie').innerHTML =parseInt(tie)+ parseInt(curTie);
    document.getElementById('lose').innerHTML =parseInt(lose)+ parseInt(curLose);
  },
  Hello: function () {
    window.alert("Hello, world!");
  },

  HelloString: function (str) {
    window.alert(Pointer_stringify(str));
  },

  PrintFloatArray: function (array, size) {
    for(var i = 0; i < size; i++)
    console.log(HEAPF32[(array >> 2) + i]);
  },

  AddNumbers: function (x, y) {
    return x + y;
  },

  StringReturnValueFunction: function () {
    var returnStr = "bla";
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  BindWebGLTexture: function (texture) {
    GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
  },

});
