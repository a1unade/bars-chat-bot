![build](https://github.com/a1unade/bars-chat-bot/actions/workflows/build.yml/badge.svg)
![deploy](https://github.com/a1unade/bars-chat-bot/actions/workflows/deploy.yml/badge.svg)

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

- GraphQL

- PostgreSQL

- Apache Kafka

- Docker + Docker Compose

- GitHub Actions (CI/CD)

**[Документация](https://github.com/a1unade/bars-chat-bot/wiki/Docs)**