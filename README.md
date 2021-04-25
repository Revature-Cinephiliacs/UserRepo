# Cinephiliacs User Microservice

## Description
This microservice is part of the Cinephiliacs application. It manages all user-centric data, such as personal information and following lists. The endpoints available create, query, or manipulate that data.


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
| Description                | Type   | Path                          | Request Body | Returned | Comments |
|----------------------------|--------|-------------------------------|--------------|----------|----------|
| Get all users              | Get    | user/users                    |              | (User)   |          |
| Create new user            | Post   | user                          | (NewUser)    |          |          |
| Update user info           | Post   | user/update/{userid}          | (NewUser)    |          |          |
| Get specific user          | Get    | user/{useremail}              |              | (User)   |          |
| Delete user                | Delete | user/delete/{userid}          |              |          |          |
| Make user admin            | Post   | user/addadmin/{userid}        |              |          |          |
| Get user's age             | Get    | user/age/{userid}             |              | number   |          |
| Get user's followed movies | Get    | user/movies/{userid}          |              | string[] |          |
| Follow a movie as a user   | Post   | user/movie/{userid}/{movieid} |              |          |          |