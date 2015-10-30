var timeline = new function() {
  this.PAINTER = "Rembrandt";
  this.drawee;

  this.draw = function() {
    console.log(this.PAINTER + " draws " + this.drawee + "");
  }

  this.setDrawee = function(who) {
    this.drawee = who;
  }
}

timeline.setDrawee(["me", "you"]);
timeline.draw();
