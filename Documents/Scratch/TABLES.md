## Tables

**key** | *foreingKey*

### Users
- **id**: _int_
- idGoogle:
- username: _string_
- creationDate: _date_
- badge: _int_
- experience: _int_
- private: _bool_
- active: _bool_
- description: _string_
- link1: _string_
- link2: _string_
- link3: _string_

### Follows
- **id**: _int_
- *idFollowed*: _int_
- *idFollowing*: _int_
- followDate: _date_
- allowed: _bool_
- allowDate: _bool_
- active: _bool_

### JobApplications
- **id**: _int_
- *userId*: _int_
- *firsStepId*: _int_
- appliedOn: _string_
- applyDate: _date_
- role: _string_
- gotIt: _bool_
- company: _string_
- description: _string_
- deleted: _bool_

### ApplicationSteps
- **id**: _int_
- *jobApId*: _int_
- *nextStepId*: _int_
- type: _string_
- description: _string_
- final: _bool_
- stepDate: _date_
- resultDate: _date_
- progressed: _bool_
- deleted: _bool_

### Reactions
- **id**: _int_
- *userId*: _int_
- *jobApId*: _int_
- *apStepId*: _int_
- type: _int_
- reaction: _int_
- reactionDate: _date_
- deleted: _bool_