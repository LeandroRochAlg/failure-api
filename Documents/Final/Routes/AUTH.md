### Authentication

#### `POST /api/auth/register`
- **Description:** Registers a new user.
- **Request Body:**
  - `username` (string, required): The desired username for the new user.
  - `password` (string, required): The password for the new user.
  - `idGoogle` (string, required): The Google ID associated with the new user.
  - `email` (string, required): The email address of the user.
  - `tokenGoogle` (string, optional): The Google token for validating the user's Google account.
- **Response:**
  - **200 OK:** User registered successfully.
  - **409 Conflict:** User with the same username or Google ID already exists.
  - **400 Bad Request:** Invalid data or error during registration.
  - **404 Not Found:** Internal server error. User not found after creation.

**Example:**
```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "bottas77",
  "password": "Bottas77!",
  "idGoogle": "google-id4",
  "email": "bottas77@example.com",
  "tokenGoogle": "google-token"
}
```

**Response:**
```http
200 OK
```

#### `POST /api/auth/login`
- **Description:** Logs in a user.
- **Request Body:**
  - `username` (string, required): The username of the user.
  - `password` (string, required): The password of the user.
  - `rememberMe` (boolean, optional): Whether to remember the user on this device.
- **Response:**
  - **200 OK:** User logged in successfully.
  - **423 Locked:** User is locked out.
  - **403 Forbidden:** User is not allowed to sign in.
  - **401 Unauthorized:** Invalid login attempt.

**Example:**
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "testuser2",
  "password": "Password123!",
  "rememberMe": true
}
```

**Response:**
```http
200 OK
```

#### `POST /api/auth/logout`
- **Description:** Logs out the current user.
- **Response:**
  - **200 OK:** User logged out successfully.

**Example:**
```http
POST /api/auth/logout
```

**Response:**
```http
200 OK
```

#### `POST /api/auth/google`
- **Description:** Logs in a user using a Google token.
- **Request Body:**
  - `idGoogle` (string, required): The Google ID of the user.
  - `tokenGoogle` (string, required): The Google token for validating the user's Google account.
- **Response:**
  - **200 OK:** User logged in successfully.
  - **400 Bad Request:** Invalid Google token.
  - **404 Not Found:** User not found.

**Example:**
```http
POST /api/auth/google
Content-Type: application/json

{
  "idGoogle": "google-id4",
  "tokenGoogle": "google-token"
}
```

**Response:**
```http
200 OK
```

### Summary of Endpoints

| HTTP Method | Route                     | Description                                 |
|-------------|---------------------------|---------------------------------------------|
| `POST`      | `/api/auth/register`      | Registers a new user                        |
| `POST`      | `/api/auth/login`         | Logs in a user                              |
| `POST`      | `/api/auth/logout`        | Logs out the current user                   |
| `POST`      | `/api/auth/google`        | Logs in a user using a Google token         |
| `GET`       | `/api/secure/check`       | Checks if the user is authenticated         |

### Additional Notes
- **Validation:** Ensure proper validation of input data, especially for registration and login endpoints (`POST /register` and `POST /login`).
- **Authentication:** Routes that require authentication should be protected with appropriate authentication mechanisms (e.g., cookies, JWT tokens).