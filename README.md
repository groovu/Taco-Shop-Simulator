# Taco Shop Simulator
A taco shop management game.
![image](https://user-images.githubusercontent.com/43489192/119241156-50d26e80-bb09-11eb-9c90-fbe5ae636be0.png)

![image](https://user-images.githubusercontent.com/43489192/119241141-3a2c1780-bb09-11eb-8a37-7d7a7ea995a1.png)


# Todo
Admin View implemented, add functionality (update users, add ingredients, add recipes)

Figure out how to use an online database.  (How to migrate local db?)

Implement buy menu

Implement research menu

Implement hiring / training menu

Leaderboard?

# Tables used

login - username, password, and privledges

userdata - username, money, employees, ...

ingredients - ingredient, description

recipes - name, ingredients required (how do you list them in a db?)

# Game Logic
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


