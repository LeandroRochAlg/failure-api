Here's the updated documentation with the routes for changing the username and password included:

### User Profile Management

#### `GET /api/users/{username}`
- **Description:** Retrieves user profile information by username.
- **Parameters:**
  - `username` (string, required): The username of the user.
- **Response:**
  - **200 OK:** Returns user profile information.
  - **404 Not Found:** User not found or inactive.

**Example:**
```http
GET /api/users/johndoe
```

**Response:**
```json
{
  "userName": "johndoe",
  "creationDate": "2023-01-01T00:00:00Z",
  "badge": 10,
  "experience": 1000,
  "private": false,
  "description": "This is a description.",
  "link1": "http://example.com",
  "link2": "http://example2.com",
  "link3": "http://example3.com"
}
```

#### `PUT /api/users/me`
- **Description:** Updates user profile information. Requires authentication.
- **Request Body:**
  - `description` (string, optional): The user's description.
  - `link1` (string, optional): The first link.
  - `link2` (string, optional): The second link.
  - `link3` (string, optional): The third link.
- **Response:**
  - **200 OK:** User profile updated successfully.
  - **400 Bad Request:** Validation errors or update failed.
  - **404 Not Found:** User not found or inactive.

**Example:**
```http
PUT /api/users/me
Content-Type: application/json

{
  "description": "Updated description.",
  "link1": "http://newlink.com",
  "link2": "http://newlink2.com",
  "link3": "http://newlink3.com"
}
```

**Response:**
```http
200 OK
```

#### `PATCH /api/users/me/privacy`
- **Description:** Toggles user privacy settings. Requires authentication.
- **Response:**
  - **200 OK:** Privacy setting toggled successfully.
  - **400 Bad Request:** Update failed.
  - **404 Not Found:** User not found or inactive.

**Example:**
```http
PATCH /api/users/me/privacy
```

**Response:**
```http
200 OK
```

#### `PATCH /api/users/me/active`
- **Description:** Toggles user active status. Requires authentication.
- **Response:**
  - **200 OK:** Active status toggled successfully.
  - **400 Bad Request:** Update failed.
  - **404 Not Found:** User not found.

**Example:**
```http
PATCH /api/users/me/active
```

**Response:**
```http
200 OK
```

#### `PUT /api/users/me/username`
- **Description:** Changes the username of the authenticated user. Requires authentication.
- **Request Body:**
  - `newUsername` (string, required): The new username to be set.
- **Response:**
  - **200 OK:** Username changed successfully.
  - **404 Not Found:** User not found or inactive.
  - **400 Bad Request:** Validation errors or update failed.

**Example:**
```http
PUT /api/users/me/username
Content-Type: application/json

{
  "newUsername": "newUsername123"
}
```

**Response:**
```http
200 OK
```

#### `PUT /api/users/me/password`
- **Description:** Changes the password of the authenticated user. Requires authentication.
- **Request Body:**
  - `currentPassword` (string, required): The user's current password.
  - `newPassword` (string, required): The new password to be set.
  - `logout` (boolean, optional): Indicates if the user should be logged out after the password change.
- **Response:**
  - **200 OK:** Password changed successfully. (User logged out if specified)
  - **404 Not Found:** User not found or inactive.
  - **400 Bad Request:** Validation errors or update failed.

**Example:**
```http
PUT /api/users/me/password
Content-Type: application/json

{
  "currentPassword": "OldPassword123!",
  "newPassword": "NewPassword123!",
  "logout": true
}
```

**Response:**
```http
200 OK
```

### Summary of Endpoints

| HTTP Method | Route                     | Description                             |
|-------------|---------------------------|-----------------------------------------|
| `GET`       | `/api/users/{username}`   | Retrieves user profile by username      |
| `PUT`       | `/api/users/me`           | Updates user profile information        |
| `PATCH`     | `/api/users/me/privacy`   | Toggles privacy settings                |
| `PATCH`     | `/api/users/me/active`    | Toggles active status                   |
| `PUT`       | `/api/users/me/username`  | Changes the username of the user        |
| `PUT`       | `/api/users/me/password`  | Changes the password of the user        |

### Additional Notes
- **Authentication:** Routes requiring authentication (`PUT`, `PATCH /privacy`, `PATCH /active`, `PUT /username`, `PUT /password`) can only be accessed by authenticated users.
- **Input Validation:** Ensure proper validation of input data, especially for update endpoints (`PUT`).