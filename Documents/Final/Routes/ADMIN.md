### Admin Management Documentation

#### `POST /api/admin/addUser/{username}`
- **Description:** Assigns admin role to a user.
- **Parameters:**
    - `username` (string, required): The username of the user to be promoted to admin.
- **Response:**
    - **200 OK:** User is successfully promoted to admin.
    - **404 Not Found:** User not found.
    - **400 Bad Request:** User already has the admin role.

**Example:**
```http
POST /api/admin/addUser/bottas77
```

**Response:**
```http
200 OK
```

#### `POST /api/admin/removeUser/{username}`
- **Description:** Revokes admin role from a user.
- **Parameters:**
    - `username` (string, required): The username of the user to have admin rights revoked.
- **Response:**
    - **200 OK:** User is successfully demoted from admin.
    - **404 Not Found:** User not found.
    - **400 Bad Request:** User is not an admin.

**Example:**
```http
POST /api/admin/removeUser/testuser2
```

**Response:**
```http
200 OK
```

### Summary of Admin Endpoints

| HTTP Method | Route                                | Description                              |
|-------------|--------------------------------------|------------------------------------------|
| `POST`      | `/api/admin/addUser/{username}`      | Assigns admin role to a user             |
| `POST`      | `/api/admin/removeUser/{username}`   | Revokes admin role from a user           |

### Additional Notes
- **Authentication:** Only users with the `Admin` role can access these routes.
- **Input Validation:** Ensure that the username provided exists in the system.