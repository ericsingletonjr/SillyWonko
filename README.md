# Silly Wonko, a most curious shoppe
Silly Wonko is a candy-inspired full E-commerce website. Users will have a delicious experience and engage in this sweet sweet project. 

# Authentication and Authorization
This site makes use of the Identity system in ASP.NET. Users can create accounts and explore various sections of the website. There are a few
claim-based policies as well as a role-based policy that we are using in this iteration. 

## Role-based Policy
Right now there exists an Admin role policy that allows admins to perform full CRUD operations with products for the site. As the site develops,
we will add more member specific policies.

## Claim-based Policies
There exist a few claim-based policies within this site. Right now, a user can register for an account and are automatically thrown into a raffle
that will determine whether or not they become a Cricket Member. There are three possible roles
* Golden Cricket Member
* Silver Cricket Member
* Bronze Cricket Member

Each of these have special landing pages they will be able to navigate to but these pages are currently blanked. They are based on the programming
challenge FizzBuzz and as a user, you'll have to use your imagination to find out if you have a golden cricket.

These claim-based policies and role-based policies are all enforced currently.

## OAuth Providers
This site implements third-party OAuth providers for users to connect existing accounts as a useable account. The two we have selected are Google and
Facebook. You will then be redirected to also include your first and last name as well as an email to register as a user on our site. If you already
have that email in the system, you will need to provide a different email to associate the google or facebook account with.

## Current DB Schema
In our database, we have various tables that make up the whole experience. This contains Product, Cart, CartItem, Order, SoldProduct as well as the
database schema that is created with the use of Identity. Our Product table is a standard inventory designed schema.

For our Cart system we have two tables:
Cart Table -
* ID
* UserID
* IsCheckedOut

CartItem Table -
* ID
* CartID
* ProductID
* Quantity

This allows us to connect a cartItem to a specific cart which is then attached to the user. Each registered user is given a cart and at the completion
of an order, this cart is then removed and they are given a brand new cart. Our order system follows a similar schema:
Order Table - 
* ID
* UserID
* IsCheckedOut
* TotalPrice

SoldProduct Table -
* ID
* ProductID
* OrderID
* Quantity

This allows us to maintain a record of a created purchase while being able to clean up our active Cart and CartItems table. All of these have the
ability to use full CRUD operations but are dependent on if a user is of role type admin.

# Vulnerability Report

With this project, we have also incuded a vulnerability report based on some of the top 10 OWASPs. You can access that document [here](VulnerabilityReport.md)

# Deployed Site
[Silly Wonko](http://sillywonko.azurewebsites.net/) is the link to the deployed site

# Contributors
* Eric Singleton
* Jermaine Walker