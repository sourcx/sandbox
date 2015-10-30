function doStuffs(person) {
  return new Promise(function(resolve, reject) {
    console.log("I promise to do it " + person + "!");

    rand = Math.random();
    if (rand < 0.5) {
      resolve(rand);
    } else {
      reject(Error("sorry, it was too small " + person));
    }
  });
}

function good(response) {
  console.log("that's good");
  console.log("because it was " + response);
}

function bad(error) {
  console.log("that's bad");
  console.log(error);
}

doStuffs("simon").then(good, bad);
