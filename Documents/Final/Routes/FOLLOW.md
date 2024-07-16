### Follow Management

#### `POST /api/follow/follow/{username}`
- **Description:** Follows a user.
- **Parameters:**
  - `username` (string, required): The username of the user to follow.
- **Response:**
  - **200 OK:** Follow request sent or user followed successfully.
  - **404 Not Found:** User not found or inactive.
  - **400 Bad Request:** User is already following the target user.

**Example:**
```http
POST /api/follow/follow/testuser2
```

**Response:**
```http
200 OK
```

#### `DELETE /api/follow/unfollow/{username}`
- **Description:** Unfollows a user.
- **Parameters:**
  - `username` (string, required): The username of the user to unfollow.
- **Response:**
  - **200 OK:** User unfollowed successfully.
  - **404 Not Found:** User not found, inactive, or not following the target user.

**Example:**
```http
DELETE /api/follow/unfollow/bottas77
```

**Response:**
```http
200 OK
```

#### `PATCH /api/follow/allow/{username}`
- **Description:** Allows a follow request.
- **Parameters:**
  - `username` (string, required): The username of the user to allow.
- **Response:**
  - **200 OK:** User allowed successfully.
  - **404 Not Found:** User not found, inactive, or not following the target user.
  - **400 Bad Request:** User is already allowed.

**Example:**
```http
PATCH /api/follow/allow/bottas77
```

**Response:**
```http
200 OK
```

#### `PATCH /api/follow/disallow/{username}`
- **Description:** Disallows a follow request.
- **Parameters:**
  - `username` (string, required): The username of the user to disallow.
- **Response:**
  - **200 OK:** User disallowed successfully.
  - **404 Not Found:** User not found, inactive, or not following the target user.
  - **400 Bad Request:** User is already disallowed.

**Example:**
```http
PATCH /api/follow/disallow/bottas77
```

**Response:**
```http
200 OK
```

#### `PATCH /api/follow/deny/{username}`
- **Description:** Denies a follow request.
- **Parameters:**
  - `username` (string, required): The username of the user to deny.
- **Response:**
  - **200 OK:** User denied successfully.
  - **404 Not Found:** User not found, inactive, or not following the target user.
  - **400 Bad Request:** User is already allowed.

**Example:**
```http
PATCH /api/follow/deny/bottas77
```

**Response:**
```http
200 OK
```

#### `GET /api/follow/followers`
- **Description:** Retrieves the list of followers.
- **Response:**
  - **200 OK:** Returns a list of followers.
  - **404 Not Found:** User not found or inactive.

**Example:**
```http
GET /api/follow/followers
```

**Response:**
```json
[
  "follower1",
  "follower2",
  "follower3"
]
```

#### `GET /api/follow/following`
- **Description:** Retrieves the list of users the authenticated user is following.
- **Response:**
  - **200 OK:** Returns a list of following users.
  - **404 Not Found:** User not found or inactive.

**Example:**
```http
GET /api/follow/following
```

**Response:**
```json
[
  "following1",
  "following2",
  "following3"
]
```

#### `GET /api/follow/requests`
- **Description:** Retrieves the list of follow requests.
- **Response:**
  - **200 OK:** Returns a list of follow requests.
  - **404 Not Found:** User not found or inactive.

**Example:**
```http
GET /api/follow/requests
```

**Response:**
```json
[
  "request1",
  "request2",
  "request3"
]
```

### Summary of Endpoints

| HTTP Method | Route                               | Description                         |
|-------------|-------------------------------------|-------------------------------------|
| `POST`      | `/api/follow/follow/{username}`     | Follows a user                      |
| `DELETE`    | `/api/follow/unfollow/{username}`   | Unfollows a user                    |
| `PATCH`     | `/api/follow/allow/{username}`      | Allows a follow request             |
| `PATCH`     | `/api/follow/disallow/{username}`   | Disallows a follow request          |
| `PATCH`     | `/api/follow/deny/{username}`       | Denies a follow request             |
| `GET`       | `/api/follow/followers`             | Retrieves the list of followers     |
| `GET`       | `/api/follow/following`             | Retrieves the list of following     |
| `GET`       | `/api/follow/requests`              | Retrieves the list of follow requests |

### Additional Notes
- **Authentication:** All routes require authentication and can only be accessed by authenticated users.
- **Input Validation:** Ensure proper validation of input data for all endpoints.