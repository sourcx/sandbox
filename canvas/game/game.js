var canvas = CE.defines("canvas").extend(Input).ready(function() {
  canvas.Scene.call("MyScene");
});

canvas.Scene.new({
  name: "MyScene",
  materials: {
    images: {
      // For CanvasEngine load "bar" first, we add index property
      "bar": {path: "http://rsamaium.github.io/CanvasEngine/samples/preload/images/bar_full.jpg", index: 0},
      "1": "http://rsamaium.github.io/CanvasEngine/samples/preload/images/1.jpg",
      "2": "http://rsamaium.github.io/CanvasEngine/samples/preload/images/2.jpg",
      "3": "http://rsamaium.github.io/CanvasEngine/samples/preload/images/3.jpg",
      warrior: "resources/images/warrior.png"
    }
  },

  called: function(stage) {
    this.loadingBar = this.createElement();
    stage.append(this.loadingBar);
  },

  preload: function(stage, pourcent, material) {
    this.loadingBar.drawImage("bar", 0, 0, pourcent + "%");
  },

  ready: function(stage) {
    this.warrior = this.createElement();
    this.warrior.drawImage("warrior");
    stage.append(this.warrior);
  },

  render: function(stage) {
    // this.warrior.x += 1;
    canvas.Input.keyDown(Input.A, function(e) {
      console.log(canvas);
      console.log(canvas.Scene.getScene("MyScene"));
      // console.log(self.warrior);
      // this.warrior.x += 1;
    });
    stage.refresh();
  }
});
