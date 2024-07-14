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

### Summary of Endpoints

| HTTP Method | Route                     | Description                            |
|-------------|---------------------------|----------------------------------------|
| `GET`       | `/api/users/{username}`   | Retrieves user profile by username     |
| `PUT`       | `/api/users/me`           | Updates user profile information       |
| `PATCH`     | `/api/users/me/privacy`   | Toggles privacy settings               |
| `PATCH`     | `/api/users/me/active`    | Toggles active status                  |

### Additional Notes
- **Authentication:** Routes requiring authentication (`PUT`, `PATCH /privacy`, `PATCH /active`) can only be accessed by authenticated users.
- **Input Validation:** Ensure proper validation of input data, especially for update endpoints (`PUT`).