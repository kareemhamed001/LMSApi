<h1>LMS API</h1>
<hr>
<p>
Welcom to LMS API documentation. 
This API is designed to provide a simple and easy to use interface for managing learning
management system.
</p>
<h2>Api Features</h2>
<ul>
<li>Users Authentication</li>
<li>Users Authentication using permissions and roles</li>
<li>CRUD operations for instructors</li>
<li>CRUD operations for courses</li>
<li>CRUD operations for lessons</li>
<li>CRUD operations for lessons contents with multiple content type</li>
<li>CRUD operations for students</li>
<li>CRUD operations for enrollments</li>
</ul>


<h2>API Endpoints</h2>
<ul>
<li>Users Authentication</li>
<ul>
<li>POST /api/v1/auth/login</li>
<li>POST /api/v1/auth/register</li>
</ul>
<li>Instructors</li>
<ul>
<li>GET /api/v1/instructors</li>
<li>POST /api/v1/instructors</li>
<li>GET /api/v1/instructors/:id</li>
<li>PUT /api/v1/instructors/:id</li>
<li>DELETE /api/v1/instructors/:id</li>
</ul>
<li>Courses</li>
<ul>
<li>GET /api/v1/courses</li>
<li>POST /api/v1/courses</li>
<li>GET /api/v1/courses/:id</li>
<li>PUT /api/v1/courses/:id</li>
<li>DELETE /api/v1/courses/:id</li>
</ul>
<li>Lessons</li>
<ul>
<li>GET /api/v1/lessons</li>
<li>POST /api/v1/lessons</li>
<li>GET /api/v1/lessons/:id</li>
<li>PUT /api/v1/lessons/:id</li>
<li>DELETE /api/v1/lessons/:id</li>
</ul>
<li>Lessons Contents</li>
<ul>
<li>GET /api/v1/lessons-contents</li>
<li>POST /api/v1/lessons-contents</li>
<li>GET /api/v1/lessons-contents/:id</li>
<li>PUT /api/v1/lessons-contents/:id</li>
<li>DELETE /api/v1/lessons-contents/:id</li>
</ul>
<li>Students</li>
<ul>
<li>GET /api/v1/students</li>
<li>POST /api/v1/students</li>
<li>GET /api/v1/students/:id</li>
<li>PUT /api/v1/students/:id</li>
<li>DELETE /api/v1/students/:id</li>
</ul>
<li>Enrollments</li>
<ul>
<li>GET /api/v1/enrollments</li>
<li>POST /api/v1/enrollments</li>
<li>GET /api/v1/enrollments/:id</li>
<li>PUT /api/v1/enrollments/:id</li>
<li>DELETE /api/v1/enrollments/:id</li>
</ul>
</ul>