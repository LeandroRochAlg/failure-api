### Reaction Management

#### `POST /api/reaction/react`
- **Description:** Adds or updates a reaction to a job application or application step.
- **Request Body:**
  - `reaction` (object, required): The reaction object containing:
    - `ReactionType` (string): Type of reaction ("Job" or other).
    - `JobApplicationId` (int): ID of the job application (if applicable).
    - `ApplicationStepId` (int): ID of the application step (if applicable).
    - `ReactionName` (string): Name of the reaction, must be one of "Like", "Congrats", "Love", "Hate", "Neutral", "Haha", "Good Luck".
- **Response:**
  - **200 OK:** Reaction added or updated successfully.
  - **400 Bad Request:** Invalid reaction name or both `JobApplicationId` and `ApplicationStepId` provided or missing.
  - **401 Unauthorized:** User not found or inactive.
  - **404 Not Found:** Job Application or Application Step not found.

**Example:**
```http
POST /api/reaction/react
Content-Type: application/json

{
    "ReactionType": "Job",
    "JobApplicationId": 1,
    "ReactionName": "Like"
}
```

**Response:**
```http
200 OK
```

#### `PATCH /api/reaction/delete`
- **Description:** Deletes a reaction to a job application or application step.
- **Request Body:**
  - `reaction` (object, required): The reaction object containing:
    - `ReactionType` (string): Type of reaction ("Job" or other).
    - `JobApplicationId` (int): ID of the job application (if applicable).
    - `ApplicationStepId` (int): ID of the application step (if applicable).
- **Response:**
  - **200 OK:** Reaction deleted successfully.
  - **400 Bad Request:** Both `JobApplicationId` and `ApplicationStepId` provided or missing.
  - **401 Unauthorized:** User not found or inactive.
  - **404 Not Found:** Job Application, Application Step, or Reaction not found.

**Example:**
```http
PATCH /api/reaction/delete
Content-Type: application/json

{
    "ReactionType": "Job",
    "JobApplicationId": 1,
    "ReactionName": "Like"
}
```

**Response:**
```http
200 OK
```

### Summary of Endpoints

| HTTP Method | Route                          | Description                       |
|-------------|--------------------------------|-----------------------------------|
| `POST`      | `/api/reaction/react`          | Adds or updates a reaction        |
| `PATCH`     | `/api/reaction/delete`         | Deletes a reaction                |

### Additional Notes
- **Authentication:** All routes require authentication and can only be accessed by authenticated users.
- **Input Validation:** Ensure proper validation of input data for all endpoints, particularly for `ReactionName` and exclusive usage of `JobApplicationId` or `ApplicationStepId`.