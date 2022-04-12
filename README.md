# BaraakuMiniBanking

## Overview

<p>This is a Demo Implementation Of Baraaku Mini Banking</p>
<p> This project Uses latest .NET framework( .NET 6).
<p>This Project adopt Containerization.</p>
The Controller Implementation takes inspiration from Steve Ardalis API Endpoint Implemetation, <br>
this API Implementation
<p>Uses Paystack Transfer Api</p>

### Authentication

<p> This Implementation makes use of Simple Basic Auth </p>
<p> Email serves as UserName and Password Remains Password </p>
<p> Every endpoint in Transaction Modules requires Auth </p>

## Assumptions

<p> Following assumptions Were made during development </p>
    
    * For TopUp endpoint 
      * User have Already been debited by calling pastack Ui portal for transfer
      * Only Notification of that Action would be sent to the backend(success or fail)

## How To Run

 * Switch to project root directory where docker compose file is located

 * <p> Run the following command docker compose up </p>
  
    > docker compose up
  
 * <p> generated swagger file can be found on this Url </p>
    
    > http://localhost/swagger/index.html
 
  * Create Account by calling this endpoint
    
    > http://localhost/api/account
  
 * Check the created Account On
    
    > http://localhost/api/account/{email}
    
 * <p>Get Account balance for SignedIn User</p>
    
    > http://localhost/api/balance
  
 ** Transfer, Top Up and Get Balance Require Authenticatiom **
 
 * Topup Endpoint 
  
    > localhost/api/Transaction/TopUp

 * Transfer Endpoint
     
     > localhost/api/Transaction/Transfer
