var darkGrey = "#6A6A6A",
    lightGrey = "#D0CFCF",
    darkRed = "#D61146",
    yearFont = "16px Verdana",
    titleFont = "14px Verdana",
    dateFont = "100 12px Sans-serif";

var width = 1000,
    height = 400,
    yearWidth = 50,
    eventBallRadius = 6;

function drawYears(c) {
  c.font = yearFont;
  c.fillStyle = lightGrey;
  c.fillText("2002", 2, 206);
  c.fillText("2015", width - yearWidth + 5, 206);
}

function drawTimeLine(c) {
  c.lineWidth = 2;
  c.strokeStyle = darkGrey;
  c.beginPath();
  c.moveTo(yearWidth, height / 2);
  c.lineTo(width - yearWidth, height / 2);
  c.stroke();
  c.moveTo(yearWidth, height / 2 - 7);
  c.lineTo(yearWidth, height / 2 + 7);
  c.stroke();
  c.moveTo(width - yearWidth, height / 2 - 7);
  c.lineTo(width - yearWidth, height / 2 + 7);
  c.stroke();
  c.closePath();
}

function drawEvent(c, percentage, title, date, lineHeight, position) {
  var textHeight = 15;

  if (position == 'bottom') {
    c.strokeStyle = darkGrey;
  } else {
    c.strokeStyle = darkRed;
    lineHeight = lineHeight * -1;
    textHeight = textHeight * -1;
  }
  c.beginPath();
  c.moveTo(yearWidth + width / 100.0 * percentage, height / 2 + eventBallRadius / 2);
  c.lineTo(yearWidth + width / 100.0 * percentage, height / 2 + eventBallRadius / 2 + lineHeight);
  c.stroke();
  c.closePath();
  c.beginPath();
  c.arc(yearWidth + width / 100.0 * percentage, height / 2, eventBallRadius, 0, 2 * Math.PI, false);
  c.fillStyle = '#fff';
  c.fill();
  c.stroke();

  c.font = titleFont;
  if (position == 'bottom') {
    c.fillStyle = darkGrey;
  } else {
    c.fillStyle = darkRed;
  }
  // c.fillText(title, yearWidth + width / 100.0 * percentage, 206);
  var titleTextWidth = 40;
  var x = yearWidth + width / 100.0 * percentage - 5,
      y = height / 2 + lineHeight - textHeight;

  y = fillTextMultiLine(c, title, x, y, titleFont);
  fillTextMultiLine(c, date, x, y, dateFont);
  // c.fillText("2015", width - yearWidth + 5, 206);
}

function fillTextMultiLine(ctx, text, x, y, font) {
  ctx.font = font;
  ctx.textAlign = "end";

  var lineHeight = ctx.measureText("M").width * 1.2;
  var lines = text.split("\n");
  for (var i = 0; i < lines.length; ++i) {
    ctx.fillText(lines[i], x, y);
    y += lineHeight;
  }
  return y;
}

function draw() {
  var canvas = document.getElementById('drawingboard');

  if (!canvas.getContext) {
    return;
  }

  var context = canvas.getContext('2d');
  drawYears(context);
  drawTimeLine(context);
  drawEvent(context, 1, 'ASP\n.NET', 'Jan 2002', 60, 'bottom');
  drawEvent(context, 3.5, 'Founded', 'Jul 2002', 80, 'top');
  drawEvent(context, 10, 'Adobe CS', 'Sep 2003', 55, 'bottom');
  drawEvent(context, 15, 'First\nMultinational\nClient', 'Apr 2004', 110, 'top');
  drawEvent(context, 16.5, 'Ruby\non Rails\nPHP 5', 'Jul 2004', 100, 'bottom');
  drawEvent(context, 20, 'Firefox', 'Nov 2004', 170, 'bottom');
  drawEvent(context, 26, 'Twitter', 'Jul 2006', 60, 'bottom');
  drawEvent(context, 27.5, 'jQuery', 'Aug 2006', 150, 'bottom');
  drawEvent(context, 32, 'First\nwebsite\nin Rails', 'Jan 2007', 70, 'top');
  drawEvent(context, 35, 'Visit\nGoogle\nUSA', 'Apr 2007', 180, 'top');
  drawEvent(context, 36.5, 'iPhone', 'Jun 2007', 100, 'bottom');
  drawEvent(context, 38, 'CSS 2.1', 'Jul 2007', 170, 'bottom');
  drawEvent(context, 45, 'Google\nChrome', 'Sep 2008', 55, 'bottom');
  drawEvent(context, 49, 'Final Cut\nPro', 'Jul 2009', 105, 'bottom');
  drawEvent(context, 50.5, 'Video\nProduction', 'Oct 2009', 55, 'top');
  drawEvent(context, 52, 'Moved to\nAmsterdam', 'Nov 2009', 115, 'top');
  drawEvent(context, 53.5, 'Git\nversioning', 'Jan 2010', 180, 'top');
  drawEvent(context, 56, 'iPad', 'Mar 2010', 140, 'bottom');
  drawEvent(context, 61, 'Backbone', 'Oct 2010', 190, 'bottom');
  drawEvent(context, 66, 'Electric\ncars', 'Jan 2011', 60, 'top');
  drawEvent(context, 72, 'First\niPhone\napp', 'May 2011', 150, 'top');
  drawEvent(context, 76, 'Tesla\nModel S', 'Jun 2012', 100, 'bottom');
  drawEvent(context, 79, 'Cloud\nhosting', 'Aug 2012', 100, 'top');
  drawEvent(context, 80.5, 'DJI\nPhantom', 'Jan 2013', 160, 'bottom');
  drawEvent(context, 82.5, 'Television\ncommercials', 'Mar 2013', 185, 'top');
  drawEvent(context, 87, 'Slack', 'Aug 2013', 60, 'bottom');
  drawEvent(context, 89, 'TinyJPG', 'Nov 2014', 60, 'top');

}
