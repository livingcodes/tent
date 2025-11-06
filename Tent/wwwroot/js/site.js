var doc = document;
var Ŏ = (qry) => doc.querySelectorAll(qry);
var Ŏ1 = (qry) => doc.querySelector(qry);
Element.prototype.on = function(evtNm, fn) {
  this.addEventListener(evtNm, fn);
}

/** Gets the index of the first occurance of the character 
  starting at but not including the specified index
  and then going backwards to the beginning of the string. */
String.prototype.revIdx = function(i, char) {
  var offset = 1;
  var found = false;
  var j = -1;
  do {
    j = i - offset;
    var curChar = this.substring(j, j+1);
    found = curChar == char;
    offset += 1;
  } while (!found && j >= 0 && offset < 8)
  return j;
}

/** Gets parent tag with class */
Element.prototype.rent = function(tag, cls) {
  if (!this.parentElement)
    return this;
  if (this.parentElement.tagName == tag
  &&  this.parentElement.classList.contains(cls))
    return this.parentElement;
  return this.parentElement.rent(tag, cls);
}