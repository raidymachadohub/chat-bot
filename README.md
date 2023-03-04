
# UI Chat.Boot



This project use libs below.

- Signal R.
- AutoMapper
- RabbitMQ Client

## Build

```bash
  dotnet build Chat.Bot.sln
```
## Installation Rabbit
```bash
  - Solution Items/docker-compose -f rabbitMQ.yml up -d
  - Access: http://localhost:15672/#/queues
  - UserName: guest | PassWord: guest
  - Create two queues. queue-bot and queue-app
```


