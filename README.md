# CompareBases

Программа сравнения баз данных.
Версия 1.05.06.  27.05.2016 - 08.11.2019

Автор: Иванов Василий Сергеевич
emAnt@mail.ru

Программа предоставляется по лицензии Apache License, Version 2.0
Подробнее смотри LICENSE на http://github.com/AantCoder/CompareBases/

# Важная информация

1. Пароли хранятся в открытом виде в файле настроек.
2. Программа не полно, а возможно и не корректно, выдает информацию по таблицам.
3. Скрипт на вкладке Применить:
 - Для процедур и функций вначале вставляется блок удаляющий их (if object_id('{0}') is not null drop {1} {0})
 - Разделитель GO делит запрос только в первых 10 строках (т.к. есть сложности с комментариями)
 - Две последние строки с символами GO в запросе удаляются перед выполнением 
4. В ходе работы программа создает папки Temp и Result, содержащие временные данные, которые можно удалять.
5. Список баз данных можно составить вручную, дописав в файл настроек csvn.xml блок:
```
  <ConnectionStrings>
    <item key="Любое имя, но в конце .my_base_name">
      <string>Data Source=127.0.0.1;Initial Catalog=my_base_name;Asynchronous Processing=True;Persist security info=true;User Id=my_user_name;Password=p@ssw0rd;</string>
    </item>
  </ConnectionStrings>
```
Чтобы база не была удалена утилитой импорта баз, добавьте символ ``` ` ``` в конец поля key: ```<item key="Любое имя, но в конце .my_base_name`">```
6. При сравнении таблиц колонки сортируются по алфавиту (но первой идет колонка с identity). Для сравнения с репозиторием файл с таблицей копируется и также сортируется (однако ограничения и прочее не изменяются).

