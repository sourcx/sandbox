'use strict'

const store = require('../store')

class UserRepo {
  constructor() {
    // super('age')
  }

  create (attributes, meta = null) {
    console.log('creating user')
    console.log(attributes)
    store.create('user', attributes, meta)
  }
}

module.exports = new UserRepo()
