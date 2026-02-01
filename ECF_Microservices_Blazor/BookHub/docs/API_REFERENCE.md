# API Reference

## Loan Service

 GET /api/loans
 GET /api/loans/{id}
 GET /api/loans/user/{userId}
 GET /api/loans/overdue
 POST /api/loans
 PUT /api/loans/{id}/return

## User Service

 GET /api/users
 GET /api/users/{id}
 POST /api/users/register
 POST /api/users/login
 PUT /api/users/{id}
 DELETE /api/users/{id}


## Book Service

 GET /api/books
 GET /api/books/{id}
 GET /api/books/search
 GET /api/books/category/{category}
 POST /api/books
 PUT /api/books/{id}
 POST /api/books/{id}/decrement-availability
 POST /api/books/{id}/increment-availability
