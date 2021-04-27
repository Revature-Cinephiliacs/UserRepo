# Cinephiliacs User Microservice

## Description
This microservice is part of the Cinephiliacs application. It manages all user-centric data, such as personal information and following lists. The endpoints available create, query, or manipulate that data.

## Technologies Used
* .NET 5
* ASP.NET Core
* EntityFramework Core

## API Address
http://20.45.2.119/
## Endpoint Objects
* User
  * userid: string,
  * username: string,
  * firstname: string,
  * lastname: string,
  * email: string,
  * dateofbirth: string,
  * permissions: number

* NewUser
  * username: string,
  * firstname: string,
  * lastname: string,
  * email: string,
  * dateofbirth: string

### Object usage within an endpoint is denoted by placing the object name with parenthesis: (Object)
## Endpoints
| Description                | Type   | Path                          | Request Body | Returned | Comments   |
|----------------------------|--------|-------------------------------|--------------|----------|------------|
| Get all users              | Get    | user/users                    |              | (User)   |            |
| Create new user            | Post   | user                          | (NewUser)    |          |            |
| Update user info           | Post   | user/update/{userid}          | (NewUser)    |          |            |
| Get specific user by email | Get    | user/{useremail}              |              | (User)   |            |
| Get specific user by userid| Get    | user/{userid}                 |              | (User)   |            |
| Delete user                | Delete | user/delete/{userid}          |              |          | Admin Only |
| Make user admin            | Post   | user/addadmin/{userid}        |              |          | Admin Only |
| Get user's age             | Get    | user/age/{userid}             |              | number   |            |
| Get user's followed users  | Get    | user/followlist/{userid}      |              | string[] |            |
| Follow a user as a user    | Post   | user/follow/{userid}/{userid} |              |          |            |
