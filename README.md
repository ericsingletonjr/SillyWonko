# Silly Wonko, a most curious shoppe
TSilly Wonko is a candy-inspired full E-commerce website. Users will have a delicious experience and engage in this sweet sweet project. 

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

# Deployed Site
[Silly Wonko](http://sillywonko.azurewebsites.net/) is the link to the deployed site
