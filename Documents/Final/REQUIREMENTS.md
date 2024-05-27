### Functional Requirements

1. **User Authentication and Management:**
   - Users must sign up using Google and provide a unique username.
   - Users can log in using only their Google account.
   - Users can change their username, which must remain unique.
   - Users can adjust account privacy settings during sign up.
   - Users can edit their profile, including adding or editing a description (max 300 characters) and up to three links.
   - Users can change the privacy settings of their account at any time.

2. **User Interaction and Badges:**
   - Users can follow other users, with private accounts requiring confirmation.
   - User badges are determined by interactions with other users.
   - The color of a badge is based on the user's experience level.
   - Experience points are gained by applying for jobs.
   - Successfully getting a job awards a significant amount of experience, calculated based on the number of application steps completed.

3. **Job Applications and Steps:**
   - Users can register job applications with details including the application platform, job role, application date, company name, and a description.
   - Users can add steps to job applications, selecting from predefined types (e.g., interview, challenge, group dynamic) and providing a description and date for each step.
   - A job application must start with an initial step, typically defined as the submission of a CV.
   - Each step in a job application can be marked as final, indicating no further steps are possible unless the final attribute is changed.
   - Registering the first step sets the `firstStepId` in the job application; subsequent steps update the `nextStepId` of the previous step.
   - Users can record the result of each step, including the date of the result.
   - When a step is marked as final and successful, the job application is marked as successful.
   - Users can indicate whether they got the job based on the final step result.
   - The UI should visually distinguish successful steps (highlighted in green) and unsuccessful steps (highlighted in red).

4. **Reactions and Feedback:**
   - Users can react to job applications, steps, and results of other users using predefined reactions (emoticons).
   - The application must handle reactions for both individual steps and overall job applications.

5. **Data Management and Integrity:**
   - Deleting a job application should also delete all associated steps, with a warning to the user.
   - Deleting a step should update the `nextStepId` of the preceding step to point to the step following the deleted one.
   - Private accounts should hide usernames and other sensitive information from other users.

### Non-Functional Requirements

1. **Performance:**
   - The application should handle concurrent users efficiently, ensuring minimal latency for database queries and page loads.
   - Real-time updates and interactions (e.g., following users, reacting to applications) should be processed swiftly to enhance user experience.

2. **Scalability:**
   - The system should be designed to scale horizontally to handle increasing numbers of users and data.
   - The architecture should support scaling of both the frontend (Vue.js) and backend (ASP.NET) components.

3. **Security:**
   - User authentication must be secure, leveraging Google's OAuth 2.0 mechanisms.
   - All user data, especially sensitive information like account details and job application data, must be securely stored and transmitted.
   - Privacy settings must be strictly enforced to protect user data from unauthorized access.

4. **Usability:**
   - The user interface should be intuitive and user-friendly, providing clear navigation and interaction elements.
   - Error messages and feedback should be informative, helping users understand and resolve issues quickly.

5. **Reliability:**
   - The application should be highly available with minimal downtime, ensuring users can access their accounts and data when needed.
   - Data integrity must be maintained, ensuring that user actions (e.g., adding steps, deleting applications) are correctly and consistently reflected in the database.

6. **Maintainability:**
   - The codebase should be well-documented and modular, facilitating easy updates and maintenance.
   - The system should support easy integration of new features and enhancements without significant rework.

7. **Compliance:**
   - The application must comply with relevant data protection regulations (e.g., GDPR) to ensure user privacy and data security.
   - Proper consent mechanisms should be in place for data collection and usage.