const userRepo = require('../repo/user')

function seedUser () {
  console.log('seeding user')
  userRepo.create({ name: 'Alice' })
}

function seedPost () {
  console.log('seeding post')
}

function seed () {
  console.log('lets seed the database')

  seedUser()
  seedPost()
}

module.exports = seed
