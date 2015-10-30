function Timeline(drawee) {
  this.PAINTER = "Mondriaan";
  this.drawee = drawee;

  this.draw = function() {
    console.log(this.PAINTER + " draws " + this.drawee + "");
  }

  this.bla = function() {
    console.log(this.PAINTER + " draws " + this.drawee + "");
  }
}

timeline = new Timeline(["me", "you"]);
timeline.draw();
timeline.do(test);