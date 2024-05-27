## Requirements

1. The user must sign up with Google and a username.
2. Once the account is created, they are redirected to the user page where the description and links can be added or edited.
3. The username also can be changed, but it is unique.
4. The user can change the account privacy settings during the sign up.
5. Login is made only with Google.
6. User badges are defined by how users interact with other users.
7. The color of the badge is defined by the amount of experience of the user.
8. Experience is gained by applying to jobs.
9. Getting a job gives the user a big amount of experience based on the number of steps passed.
10. Private accounts show other users nothing about their username.
11. Descriptions have a maximum of 300 characters.
12. Users may follow other users.
13. Following a private account needs a confirmation.
14. Users can register a job application with where they applied on, the job role, the application date, the company and the description.
15. Users can register application steps that can be only predefined kinds (interview, challenge, group dynamic...). A description and a date may also be given by the user.
16. A application step can be final if the user says so.
17. Registering the first step of a job application sets the firstStepId value of the job application table. Other steps keep the nextStepId value of the previous step.
18. To register a application the user need to register a application step generally defined by the sending of a CV.
19. Users can register the result of a step with the date of the result.
20. When a user registers a step, the previous step is automatically set to a successful result.
21. If a step is final, another step can't be defined without changing the final attribute of the previous step.
22. If the step is final and the result is successful, it means that the user got the job and it must be automatically registered on the job application.
23. When the result of a step is registered as not successful (false), the user is given the opportunity to register that didn’t get the job.
24. On the UI, a successful step is highlighted in green and a not successful is red. The job application is the same. A job application wraps all the steps.
25. Deleting a step transforms the nextStepId of the step prior to the deleted into the id of the posterior step of the deleted.
26. Deleting a job application deletes all its steps. This needs to be highlighted for the user when they try to delete a job application.
27. Users can react to job applications, steps, and results of other users with some predefined reactions (emoticons).