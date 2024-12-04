const store = require('./store')
const seed = require('./seed/seed')

store.on('connect', () => {
  console.log('Connected to the database')
})

store.on('failure', error => {
  console.error('Failed to connect to the database', error)
})

store
  .connect()
  .then(() => {
    console.log('Connection successful')
  })
  .then(() => seed())
  .then(() => {
    console.log('Database seeded')
  })
  .then(() => {
    return store.disconnect()
  })
  .catch(error => {
    console.error('Error:', error)
  })
