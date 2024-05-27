### User Routes
1. **User Registration and Login**
   - `POST /api/auth/register` - Registers a new user using Google OAuth and a unique username.
   - `POST /api/auth/login` - Logs in a user using Google OAuth.

2. **User Profile Management**
   - `GET /api/users/{id}` - Retrieves user profile information by user ID.
   - `PUT /api/users/{id}` - Updates user profile information (e.g., description, links, username).
   - `PATCH /api/users/{id}/privacy` - Updates user privacy settings.

3. **User Follow Management**
   - `POST /api/users/{id}/follow` - Follows another user.
   - `DELETE /api/users/{id}/unfollow` - Unfollows a user.
   - `GET /api/users/{id}/followers` - Retrieves the list of followers for a user.
   - `GET /api/users/{id}/following` - Retrieves the list of users the user is following.

### Job Application Routes
4. **Job Application Management**
   - `POST /api/jobapplications` - Creates a new job application.
   - `GET /api/jobapplications/{id}` - Retrieves a specific job application by ID.
   - `PUT /api/jobapplications/{id}` - Updates a job application.
   - `DELETE /api/jobapplications/{id}` - Deletes a job application and all associated steps.

### Application Step Routes
5. **Application Step Management**
   - `POST /api/jobapplications/{jobAppId}/steps` - Adds a new step to a job application.
   - `GET /api/jobapplications/{jobAppId}/steps/{stepId}` - Retrieves a specific step by ID.
   - `PUT /api/jobapplications/{jobAppId}/steps/{stepId}` - Updates a specific step.
   - `DELETE /api/jobapplications/{jobAppId}/steps/{stepId}` - Deletes a specific step and updates the `nextStepId` of the previous step.

6. **Step Result Management**
   - `PATCH /api/jobapplications/{jobAppId}/steps/{stepId}/result` - Registers the result of a step.

### Reaction Routes
7. **Reaction Management**
   - `POST /api/reactions` - Adds a reaction to a job application, step, or result.
   - `DELETE /api/reactions/{id}` - Deletes a specific reaction.

### Example Route Definitions

Here is a detailed list of routes and their descriptions:

#### User Routes
- **POST /api/auth/register**: Registers a new user.
  - Body: `{ "googleToken": "string", "username": "string" }`

- **POST /api/auth/login**: Logs in a user.
  - Body: `{ "googleToken": "string" }`

- **GET /api/users/{id}**: Retrieves user profile information.
  - Params: `id` (user ID)

- **PUT /api/users/{id}**: Updates user profile.
  - Params: `id` (user ID)
  - Body: `{ "description": "string", "link1": "string", "link2": "string", "link3": "string", "username": "string" }`

- **PATCH /api/users/{id}/privacy**: Updates user privacy settings.
  - Params: `id` (user ID)
  - Body: `{ "private": "boolean" }`

- **POST /api/users/{id}/follow**: Follows another user.
  - Params: `id` (user ID)

- **DELETE /api/users/{id}/unfollow**: Unfollows a user.
  - Params: `id` (user ID)

- **GET /api/users/{id}/followers**: Retrieves the list of followers.
  - Params: `id` (user ID)

- **GET /api/users/{id}/following**: Retrieves the list of users being followed.
  - Params: `id` (user ID)

#### Job Application Routes
- **POST /api/jobapplications**: Creates a new job application.
  - Body: `{ "userId": "int", "appliedOn": "string", "applyDate": "date", "role": "string", "company": "string", "description": "string" }`

- **GET /api/jobapplications/{id}**: Retrieves a job application.
  - Params: `id` (job application ID)

- **PUT /api/jobapplications/{id}**: Updates a job application.
  - Params: `id` (job application ID)
  - Body: `{ "appliedOn": "string", "applyDate": "date", "role": "string", "company": "string", "description": "string", "gotIt": "boolean" }`

- **DELETE /api/jobapplications/{id}**: Deletes a job application.
  - Params: `id` (job application ID)

#### Application Step Routes
- **POST /api/jobapplications/{jobAppId}/steps**: Adds a new step to a job application.
  - Params: `jobAppId` (job application ID)
  - Body: `{ "type": "string", "description": "string", "stepDate": "date", "final": "boolean" }`

- **GET /api/jobapplications/{jobAppId}/steps/{stepId}**: Retrieves a specific step.
  - Params: `jobAppId` (job application ID), `stepId` (step ID)

- **PUT /api/jobapplications/{jobAppId}/steps/{stepId}**: Updates a specific step.
  - Params: `jobAppId` (job application ID), `stepId` (step ID)
  - Body: `{ "type": "string", "description": "string", "stepDate": "date", "final": "boolean", "progressed": "boolean" }`

- **DELETE /api/jobapplications/{jobAppId}/steps/{stepId}**: Deletes a specific step.
  - Params: `jobAppId` (job application ID), `stepId` (step ID)

- **PATCH /api/jobapplications/{jobAppId}/steps/{stepId}/result**: Registers the result of a step.
  - Params: `jobAppId` (job application ID), `stepId` (step ID)
  - Body: `{ "resultDate": "date", "successful": "boolean" }`

#### Reaction Routes
- **POST /api/reactions**: Adds a reaction.
  - Body: `{ "userId": "int", "jobApId": "int?", "apStepId": "int?", "type": "int", "reaction": "int" }`

- **DELETE /api/reactions/{id}**: Deletes a reaction.
  - Params: `id` (reaction ID)