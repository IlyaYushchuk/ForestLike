# ForestLike
ForestLike is a productivity app. If you need to concentrate, start a timer, the desire to get a rating will motivate you to stay focused.
# Ilya Yushchuk group 253505
# Class diagram
![image](https://github.com/IlyaYushchuk/ForestLike/assets/112698602/dd34e688-64fe-4455-8412-c03829065a41)

## DataAccess layer
This layer is responsible for retrieving and storing data from a database or local storage

![image](https://github.com/IlyaYushchuk/ForestLike/assets/112698602/e3f7fc0d-50c6-40d8-834d-6ef90e989836)

### IStorageController
- interface that defines common methods for working with a data store.
### DBController 
- class implementing interface IStorageController for work with remoted data base.
### LocalStorageController 
- class implementing interface IStorageController for work with local storage. This class is required to use the application without registration.
### SerializationService 
- responsible for serializing data for writing to storage and deserializing data from storage.

## Business-Logical layer
The layer contains all the logic needed by the application to implementation of all functions. 

![image](https://github.com/IlyaYushchuk/ForestLike/assets/112698602/505a36c1-2174-45ec-9ddc-b47b86d29eab)

### User
-representing the user stores the name, hashed password, rating and ID.
### ActivityObserver
-track user activity in concentration time.
### Statistic
-class responsible for statistic. For example the most popular theme for concentration.
### Timer
-main class which start ActivityObserver and create new records of concentration.
### CooperativeTimer
-class provide ability to set timer with another user.
### Record
-store data about concentration: time, theme, date, was it failed, time of fail
### CooperativeRecord 
-same class as Record but it also store info about second user.

## Presentation layer
Layer contains all of the classes responsible for presenting the
UI/visualization to the user.

![image](https://github.com/IlyaYushchuk/ForestLike/assets/112698602/a3c43261-8eae-49ec-82f8-296d77ad9e6b)


## Functional
1) Timer. User set amout of time during which he want to be concentrated. Affter start app will track active app and if user close/swith this app his progress will be lost.
2) User are able to set stopwatch instead of timer. But if he will use restricted apps all his progress will be lost. Сoncentration time will be counted only if user stop stopwatch.
3) User can select a theme for concentration timer (example: learning, working, rest).
4) User can choose apps which he can use without fail all other will be restricted.
5) User can set easier mod in which app will set several notification if he use restricted apps before fail.
6) Registration in app. Data about the achievements of a registered user can be stored remotely. Registration add user to data base info about user (generated id, name, hashed password, rating and reg date).
7) Login in app. 
8) Logup out of app.
9) Change name and password of user.
10) User can login as guest. In this case he doesn't have to register but his achivements will stored remotely.
11) Two users can set a timer together. In this case, if one loses the timer, both will lose. This feature will give them extra motivation to stay focused. This function is available only for registred users. 
12) Every user have rating. Rating will increase when he successfully completed concentration without failure. Amount of rating will be depend of concetration time.
13) In the application user can view the best users by rating.

## Data models
![image](https://github.com/IlyaYushchuk/ForestLike/assets/112698602/001fc349-f563-4d3e-8078-710d001a6093)
### User
-contain name, rating, id, hashed password and recoeds
### Record
-contain time, theme, date, was it failed and time of fail.
### CooperativeRecord
-same as Record but also contain info about second user.
### SecondUser
-contain name, rating and id. Required to store data about the second user.

