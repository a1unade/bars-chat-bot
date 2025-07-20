![build](https://github.com/a1unade/bars-chat-bot/actions/workflows/build.yml/badge.svg)
![deploy](https://github.com/a1unade/bars-chat-bot/actions/workflows/deploy.yml/badge.svg)

## Содержание 

- [Описание проекта](#описание-проекта)

- [Как запускать проект](#как-запускать-проект)

- [API Gateway](#api-gateway)

- [GraphQL](#graphql)

    - [Queries](#queries)

        - [Users](#users)

        - [Notifications](#notifications)

    - [Mutations](#mutations)

        - [Users](#users-1)

        - [Notifications](#notifications-1)

## Описание проекта

**NotifyHub** — Чат-бот-регламентатор для напоминаний

**NotifyHub** — это Telegram-чат-бот, разработанный в рамках производственной практики в компании **БАРС ГРУП**, предназначенный для своевременного напоминания пользователю о необходимости выполнения регламентных операций.

**Возможности**:  

- Создание и управление напоминаниями

- Поддержка периодических уведомлений (день/неделя/месяц)

- Асинхронная обработка задач с использованием Kafka

- Масштабируемая микросервисная архитектура:

    - Web API для управления уведомлениями

    - Notification Service для отправки

    - Outbox Processor для планирования и очередей

    - API Gateway для маршрутизации

    - Telegram Bot в роли клиента

**Технологии**:

- .NET 8

- PostgreSQL

- Apache Kafka

- Docker + Docker Compose

- GitHub Actions (CI/CD)

### Архитектура проекта

<img src="./materials/2.jpeg" />

### Как запускать проект

Весь проект запускается с помощью `docker-compose`. 

Чтобы полностью пересобрать проект, можно воспользоваться командами: 

```shell
docker-compose down --rmi all -v
docker-compose up --build -d
```

также были добавлены сценарии для сборки через `Makefile`:

```shell
make docker
```

```shell
make docker-rebuild
```

После выполнения команд результат должен быть примерно таким: 

<img src="./materials/1.png" />

## API Gateway

Для запросов к микросервисам настроен `API Gateway` и доступен по адресу `http://localhost:8088`

Для плейграундов у микросервисов такие адреса: 

- graphql: `http://localhost:8088/graphql`

- swagger (notification service): `http://localhost:8088/swagger/index.html`

## GraphQL

### Queries

#### Users

- Получение списка всех пользователей

    Пример запроса: 

    ```graphql
    query {
        users {
            name
            id
        }
    }
    ```

    Пример ответа: 

    ```json
    "data": {
        "users": [
            {
                "name": "Иван Иванов",
                "id": "e8e60a2c-46f4-41e4-8e56-ecb97df34de2"
            },
            {
                "name": "Иван Иванов",
                "id": "ba3d2ce7-54ff-4d3b-a0ec-7bca83f21939"
            },
            {
                "name": "Иван Иванов",
                "id": "d8f05f57-c42b-4c4c-a849-f5f7d80843d8"
            },
            {
                "name": "Иван Иванов",
                "id": "b0634b92-47e2-4720-bfeb-888f336279bb"
            }
        ]
    }
    ```

- Получение пользователя по ID: 

    Пример запроса: 

    ```graphql
    query {
        users (where: { id: { eq: "d8dc0566-7ad0-47b3-83cd-9bff6c10ab16" } }) {
            name
            id
        }
    }
    ```

    Пример ответа: 

    ```json
    "data": {
        "users": [
            {
                "name": "Иван Иванов",
                "id": "d8dc0566-7ad0-47b3-83cd-9bff6c10ab16"
            }
        ]
    }
    ```

#### Notifications

- Получение списка всех уведомлений

    Пример запроса: 

    ```graphql
    query {
        notifications {
            id
            type
            frequency
            scheduledAt
            title
            description
        }
    }
    ```

    Пример ответа: 

    ```json
    "data": {
        "notifications": [
            {
                "id": "956e5542-59b2-4dc4-8de4-733528a2a72e",
                "type": "ONE_TIME",
                "frequency": null,
                "scheduledAt": "2025-08-01T09:00:00.000Z",
                "title": "notification",
                "description": "description"
            },
            {
                "id": "222e8b6a-2ba7-470b-a747-f96489649683",
                "type": "ONE_TIME",
                "frequency": null,
                "scheduledAt": "2025-08-01T09:00:00.000Z",
                "title": "notification",
                "description": "description"
            },
            {
                "id": "bae79eb1-d436-4d8d-be0a-acf210629562",
                "type": "ONE_TIME",
                "frequency": null,
                "scheduledAt": "2025-08-01T09:00:00.000Z",
                "title": "notification",
                "description": "description"
            },
            {
                "id": "06add0b3-37f7-47fb-a8fa-26395c9ec333",
                "type": "ONE_TIME",
                "frequency": null,
                "scheduledAt": "2025-08-01T09:00:00.000Z",
                "title": "notification",
                "description": "description"
            }
        ]
    }
    ```

- Получение уведомления по ID

    Пример запроса: 

    ```graphql
    query {
        notifications (where: { id: { eq: "06add0b3-37f7-47fb-a8fa-26395c9ec333" } }) {
            id
            title
            description
        }
    }
    ```

    ```json
    "data": {
        "notifications": [
            {
                "id": "06add0b3-37f7-47fb-a8fa-26395c9ec333",
                "title": "notification",
                "description": "description"
            }
        ]
    }
    ```

### Mutations

#### Users

- Создание пользователя 

    Пример запроса: 

    ```graphql
    mutation {
        createUser(
            request: {
            name: "Иван Иванов"
            telegramTag: "@ivan"
            }
        )
    }
    ```

    Пример ответа:

    ```json
    "data": {
        "createUser": "d8dc0566-7ad0-47b3-83cd-9bff6c10ab16"
    }
    ```

- Обновление пользователя

    Пример запроса: 

    ```graphql
    mutation {
        updateUser(request: {
            id: "892a8419-5d0f-49c9-9257-12b558d7b194",
            name: "Новое имя",
            email: "new.email@example.com"
        }) {
            id
            name
            email
            telegramTag
        }
    }
    ```

    Пример ответа: 

    ```json
    "data": {
        "updateUser": {
            "id": "892a8419-5d0f-49c9-9257-12b558d7b194",
            "name": "Новое имя",
            "email": "new.email@example.com",
            "telegramTag": "@ivan"
        }
    }
    ```
    
- Удаление пользователя

    Пример запроса:

    ```graphql
    mutation {
        deleteUser (id: "892a8419-5d0f-49c9-9257-12b558d7b194")
    }
    ```

    Пример ответа: 

    ```json
    "data": {
        "deleteUser": true
    }
    ```

#### Notifications

- Создание уведомления

    Пример запроса: 

    ```graphql
    mutation CreateNotification {
        createNotification(
            request: {
            userId: "d8dc0566-7ad0-47b3-83cd-9bff6c10ab16"
            title: "notification"
            description: "description"
            type: ONE_TIME
            scheduledAt: "2025-08-01T09:00:00Z"
            }
        ) {
            id
            title
            description
            type
            frequency
            scheduledAt
            email
            telegramTag
        }
    }
    ```

    Пример ответа: 

    ```json
    "data": {
        "createNotification": {
            "id": "06add0b3-37f7-47fb-a8fa-26395c9ec333",
            "title": "notification",
            "description": "description",
            "type": "ONE_TIME",
            "frequency": null,
            "scheduledAt": "2025-08-01T09:00:00.000Z",
            "email": "ivan@example.com",
            "telegramTag": "@ivan"
        }
    }
    ```

- Обновление уведомления по ID

    Пример запроса: 

    ```graphql 
    mutation {
        updateNotification(
            request: {
            id: "8f99832e-b15e-4be7-8a19-156881dbe4a2"
            title: "Новое название уведомления"
        }) {
            id
            title
            description
            type
            scheduledAt
        }
    }
    ```

    Пример ответа: 

    ```json
    "data": {
        "updateNotification": {
        "id": "8f99832e-b15e-4be7-8a19-156881dbe4a2",
        "title": "Новое название уведомления",
        "description": "description",
        "type": "ONE_TIME",
        "scheduledAt": "2025-08-01T09:00:00.000Z"
        }
    }
    ```

- Удаление уведомления по ID

    Пример запроса: 

    ```graphql
    mutation {
        deleteNotification (id: "8f99832e-b15e-4be7-8a19-156881dbe4a2")
    }
    ```

    Пример ответа: 

    ```json
    "data": {
        "deleteNotification": true
    }
    ```