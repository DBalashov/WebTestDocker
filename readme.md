## Исходные данные

Система состоит из двух контейнеров:

![](https://dl.dropboxusercontent.com/s/coa6hqf4oi8cj0f/20_203436.png)

Командная строка для сборки каждого проекта есть в dockerfile в строке RUN

### Контейнер WebTestServer
* веб-сервер, имеет три страницы
  * /Home - отображение справочной информации (переменные среды, сетевые интерфейсы, ...)
  * /Local - страница запроса курса валют с cbr.ru напрямую. WebTestServer обращается напрямую к cbr.ru и получает оттуда данные.
  * /Remote - страница запроса курса валют через обращение к сервису WebTestService
* exposит наружу один порт http/5000
* обращается к сервису по имени хоста заданного в appsettings/WebTestServiceAddress
* прилагается dockerfile для сборки контейнера
 
### Контейнер WebTestService
* сервис, единственная задача которого принять запрос от веб-сервера, получить курс валют и отдать его
* exposит наружу один порт http/3000
* прилагается dockerfile для сборки контейнера

## Задача 1
* сделать docker-compose файл для сборки комплекта
* сделать .gitlab-ci.yml для сборки проекта в gitlab pipelines

## Задача 2
* обеспечить возможность масштабирования WebTestService в режиме docker swarm в 2 или более экземпляров. Каждый запрос от вебсервера должен улетать в случайный контейнер:
![](https://dl.dropboxusercontent.com/s/6nob0wc0h36h39t/20_203700.png)


* обеспечить изоляцию контейнера WebTestService с помощью промежуточного контейнера с nginx. Запрос от вебсервера направляется в контейнер с nginx, оттуда - в контейнер с WebTestService:
![](https://dl.dropboxusercontent.com/s/hgx68iikiktiu3s/20_204456.png)


* обеспечить балансировку нагрузки не средствами docker, а средствами nginx, запустив 2 или более контейнеров с WebTestService и настроив балансировку средствами nginx:
![](https://dl.dropboxusercontent.com/s/cly31xdggrifd25/20_203718.png)


* в дополнение к предыдущему варианту - добавить nginx перед WebTestServer:
![](https://dl.dropboxusercontent.com/s/eivz20c20sy3ang/20_203850.png)

## Задача 3 (на "отлично")
* развернуть kubernetes, настроить интеграцию с gitlab
* обеспечить автоматический сборку и деплой обоих сервисов (WebTestServer и WebTestService) в kubernetes
* настроить ingress-сервис для доступа к WebTestServer