### Job Application Management

#### `POST /api/jobApplication/register`
- **Description:** Registers a new job application.
- **Request Body:**
  - `appliedOn` (string, required): The platform where the application was submitted.
  - `role` (string, required): The role applied for.
  - `company` (string, required): The company where the application was submitted.
  - `description` (string, required): A description of the application.
- **Response:**
  - **201 Created:** Job application registered successfully.
  - **400 Bad Request:** Invalid input data.

**Example:**
```http
POST /api/jobApplication/register
Content-Type: application/json

{
    "appliedOn": "LinkedIn",
    "role": "Full-Stack Developer",
    "company": "Spotify",
    "description": "Trying to get a job at Spotify"
}
```

**Response:**
```http
201 Created
```

#### `GET /api/jobApplication/list/{username}`
- **Description:** Retrieves the list of job applications for a specific user.
- **Parameters:**
  - `username` (string, required): The username of the user whose job applications are to be retrieved.
- **Response:**
  - **200 OK:** Returns a list of job applications.
  - **404 Not Found:** User not found or inactive.

**Example:**
```http
GET /api/jobApplication/list/testuser2
```

**Response:**
```json
[
  {
    "id": 1,
    "appliedOn": "LinkedIn",
    "role": "Full-Stack Developer",
    "company": "Spotify",
    "description": "Trying to get a job at Spotify"
  },
  {
    "id": 2,
    "appliedOn": "Indeed",
    "role": "Backend Developer",
    "company": "Google",
    "description": "Trying to get a job at Google"
  }
]
```

#### `POST /api/jobApplication/applicationStep`
- **Description:** Registers a new application step for a job application.
- **Request Body:**
  - `jobApplicationId` (integer, required): The ID of the job application.
  - `title` (string, required): The title of the application step.
  - `type` (string, required): The type of the application step.
  - `description` (string, required): A description of the application step.
  - `stepDate` (string, required): The date of the application step.
- **Response:**
  - **201 Created:** Application step registered successfully.
  - **400 Bad Request:** Invalid input data.

**Example:**
```http
POST /api/jobApplication/applicationStep
Content-Type: application/json

{
    "jobApplicationId": 2,
    "title": "Interview with the HR",
    "type": "Interview",
    "description": "Interview with 3 people from the HR department",
    "stepDate": "2024-07-18"
}
```

**Response:**
```http
201 Created
```

#### `PATCH /api/jobApplication/delete/{id}`
- **Description:** Soft deletes a job application.
- **Parameters:**
  - `id` (integer, required): The ID of the job application to be deleted.
- **Response:**
  - **200 OK:** Job application deleted successfully.
  - **404 Not Found:** Job application not found.

**Example:**
```http
PATCH /api/jobApplication/delete/1
```

**Response:**
```http
200 OK
```

#### `PUT /api/jobApplication/update/{id}`
- **Description:** Updates a job application.
- **Parameters:**
  - `id` (integer, required): The ID of the job application to be updated.
- **Request Body:**
  - `appliedOn` (string, required): The platform where the application was submitted.
  - `role` (string, required): The role applied for.
  - `company` (string, required): The company where the application was submitted.
  - `description` (string, required): A description of the application.
- **Response:**
  - **200 OK:** Job application updated successfully.
  - **400 Bad Request:** Invalid input data.
  - **404 Not Found:** Job application not found.

**Example:**
```http
PUT /api/jobApplication/update/1
Content-Type: application/json

{
    "appliedOn": "Gupy",
    "role": "Full-Stack Developer",
    "company": "Spotify",
    "description": "Updated description"
}
```

**Response:**
```http
200 OK
```

#### `PUT /api/jobApplication/applicationStep/progressed/{stepId}/{date}`
- **Description:** Marks an application step as progressed.
- **Parameters:**
  - `stepId` (integer, required): The ID of the application step.
  - `date` (string, required): The date when the step was progressed.
- **Response:**
  - **200 OK:** Application step progressed successfully.
  - **404 Not Found:** Application step not found.

**Example:**
```http
PUT /api/jobApplication/applicationStep/progressed/6/2024-07-20
```

**Response:**
```http
200 OK
```

#### `PUT /api/jobApplication/applicationStep/failed/{stepId}/{date}`
- **Description:** Marks an application step as failed.
- **Parameters:**
  - `stepId` (integer, required): The ID of the application step.
  - `date` (string, required): The date when the step was failed.
- **Response:**
  - **200 OK:** Application step failed successfully.
  - **404 Not Found:** Application step not found.

**Example:**
```http
PUT /api/jobApplication/applicationStep/failed/6/2024-07-20
```

**Response:**
```http
200 OK
```

#### `PATCH /api/jobApplication/applicationStep/delete/{stepId}`
- **Description:** Soft deletes an application step.
- **Parameters:**
  - `stepId` (integer, required): The ID of the application step to be deleted.
- **Response:**
  - **200 OK:** Application step deleted successfully.
  - **404 Not Found:** Application step not found.

**Example:**
```http
PATCH /api/jobApplication/applicationStep/delete/1
```

**Response:**
```http
200 OK
```

#### `PUT /api/jobApplication/success/{id}`
- **Description:** Marks a job application as successful.
- **Parameters:**
  - `id` (integer, required): The ID of the job application.
- **Response:**
  - **200 OK:** Job application marked as successful.
  - **404 Not Found:** Job application not found.

**Example:**
```http
PUT /api/jobApplication/success/1
```

**Response:**
```http
200 OK
```

#### `PATCH /api/jobApplication/applicationStep/final/{stepId}`
- **Description:** Marks an application step as final.
- **Parameters:**
  - `stepId` (integer, required): The ID of the application step to be marked as final.
- **Response:**
  - **200 OK:** Application step marked as final.
  - **404 Not Found:** Application step not found.

**Example:**
```http
PATCH /api/jobApplication/applicationStep/final/1
```

**Response:**
```http
200 OK
```

### Summary of Endpoints

| HTTP Method | Route                                                        | Description                                   |
|-------------|--------------------------------------------------------------|-----------------------------------------------|
| `POST`      | `/api/jobApplication/register`                               | Registers a new job application               |
| `GET`       | `/api/jobApplication/list/{username}`                        | Retrieves the list of job applications        |
| `POST`      | `/api/jobApplication/applicationStep`                        | Registers a new application step              |
| `PATCH`     | `/api/jobApplication/delete/{id}`                            | Soft deletes a job application                |
| `PUT`       | `/api/jobApplication/update/{id}`                            | Updates a job application                     |
| `PUT`       | `/api/jobApplication/applicationStep/progressed/{stepId}/{date}` | Marks an application step as progressed   |
| `PUT`       | `/api/jobApplication/applicationStep/failed/{stepId}/{date}` | Marks an application step as failed           |
| `PATCH`     | `/api/jobApplication/applicationStep/delete/{stepId}`        | Soft deletes an application step              |
| `PUT`       | `/api/jobApplication/success/{id}`                           | Marks a job application as successful         |
| `PATCH`     | `/api/jobApplication/applicationStep/final/{stepId}`         | Marks an application step as final            |

### Additional Notes
- **Authentication:** All routes require authentication and can only be accessed by authenticated users.
- **Input Validation:** Ensure proper validation of input data for all endpoints.