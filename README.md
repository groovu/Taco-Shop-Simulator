# Taco Shop Simulator
This is a shop management game written in C# using Windows Foundation Presentation, SQL Server, and Entity Framework.  Still a work in progress, but current it features a login/registration system, user and admin UI, purchasing inventory, and a chat system
![image](https://user-images.githubusercontent.com/43489192/119241156-50d26e80-bb09-11eb-9c90-fbe5ae636be0.png)

![image](https://user-images.githubusercontent.com/43489192/119241179-7a8b9580-bb09-11eb-8b5d-572ba5b501fa.png)


# Todo
Migrate to online database.
Implement game logic (and how do I make it fun? lol)


# Game Logic Ideas
Users run a taco shop.  Every week, the user can...
	- hire / train employees
	- move their location?
	- buy ingredients
	- advertise
	- research new recipes

The goal is to maximize profits.  Profit = Income - cost of (employees, rent, ingredients, advertisement?)
User starts with 1000 dollars and the game is over if the user's networth is negative.

Income
	- Income comes from customers.
	- You can get more customers by being in a good location (high rent) and advertising.

Employees
	- Can have stats that make them perform better (more tasks, ...)

Rent
	- Rent is a cost associated with a location.
	- Low rent locations give less customers.
	- Hi rent location give more customers.

Ingredients
	- User must purchase ingredients and have them on hand to cook recipes.
	- Ingredients will expire every week?

Advertisement
	- User can choose to buy advertisements.
	- Advertisements increase the number of customers that come to the store.

Ideas
	- Shop reputation system?  If you max out customers but can't provide them, something negative should happen.
	- How to allow for different types of play?  Slow and steady vs Fast and Risky / letting the user change from week to week.


