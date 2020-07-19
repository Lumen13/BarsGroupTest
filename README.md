[ENG]

Hi!

Before launch this app you need to do some tricks:
1. Open Package Manager Console and type: "add-migration initial"
2. Then type: "update-database"
3. Use this SQL-Query on the new "barsgrouptest" database: 
  "insert into info ("DbSize") values((select distinct pg_size_pretty(pg_database_size('barsgrouptest')) from pg_class))"
4. Click ctrl+F5 and u will have the console result, which will show exactly the same in Google Sheet

Program will make only the one Request with the one "appsettings.json" and "client_secret.json" configuration files to one Database.
In the "Startup.cs" you can see any other hided methods to do any CRUD manipulations.

Here it is
https://docs.google.com/spreadsheets/d/1emVkyWkxLp9BudpS7qsYhgBGJn8In8ZwDZGhTJ3xAf0/edit?usp=sharing

Hope you will enjoy it, even a little :-)

----------------------------------------------------------------------------------------------------------------------------

[RUS]

Доброго времени суток!

Прежде чем запустить приложение, необходимо проделать некоторые манипуляции:
1. Откройте Package Manager Console и наберите: "add-migration initial"
2. Затем наберите: "update-database"
3. Используйте SQL-Запрос к новой базе данных "barsgrouptest":
  "insert into info ("DbSize") values((select distinct pg_size_pretty(pg_database_size('barsgrouptest')) from pg_class))"
4. Нажмите ctrl+F5 и в консоли отобразится результат работы программы, который будет продублирован в Google Таблице

Программа выполнит только один Запрос с единственными конфигурационными файлами "appsettings.json" and "client_secret.json".
Запрос пройдет только к одной базе данных. 
Так же, в классе "Startup.cs" Вы можете видеть закомментированные методы, которые позволят выполнить любые CRUD манипуляции.

Вот таблица:
https://docs.google.com/spreadsheets/d/1emVkyWkxLp9BudpS7qsYhgBGJn8In8ZwDZGhTJ3xAf0/edit?usp=sharing

Надеюсь, что это, хоть немного, удовлетворит Ваши ожидания :-)
