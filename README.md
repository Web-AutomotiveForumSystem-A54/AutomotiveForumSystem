# AutomotiveForumSystem

<p id="description">The Automotive Forum Web App is a dynamic and engaging platform designed for automotive enthusiasts to connect share and explore a wide range of automotive topics. This web application provides a seamless and user-friendly experience for users passionate about cars and everything related to the automotive world.</p>

  
  
<h2>🧐 Features</h2>

Here're some of the project's best features:

*   Effortlessly explore the Automotive Forum through a well-designed sidebar for quick access to home post creation and category/tag sections.
*   Enhance searchability by enriching posts with relevant tags enabling quick identification of topics.
*   Streamline user experience with a powerful search bar for finding specific posts.
*   Stay informed about community growth with real-time updates on member and post counts.
*   Enjoy personalized interactions and track contributions securely through user authentication.

<h2>🛠️ Installation Steps:</h2>

<p>1. Clone the repository.</p>

```
git clone https://github.com/Web-AutomotiveForumSystem-A54/AutomotiveForumSystem.git
```

<p>2. Navigate to the project directory.</p>

```
cd automotive-forum
```

<p>3. Configure Database Connection: Open the appsettings.json file. Update the connection string under the "DefaultConnection" key to point to your SQL Server database.</p>

<p>4. Download and install the .NET Core SDK.</p>

<p>5. Build the Application:</p>

```
dotnet build
```

<p>6. Apply Database Migrations:</p>

```
dotnet ef database update
```

<p>7. Run the Application:</p>

```
dotnet run
```

<p>8. Navigate to http://localhost:5000 to access the Automotive Forum Web App.</p>

<p>9. Log in or create an account to explore the forum create posts and engage with the community.</p>

<p>10. If desired use SSMS to create a backup of your SQL Server database for data security.</p>

  
  
<h2>💻 Built with</h2>

Technologies used in the project:

*   ASP.NET Core
*   Razor Pages (CSHTML)
*   Bootstrap
*   jQuery
*   Entity Framework Core
*   HTML/CSS
*   JavaScript
*   SQL
*   Git

<h2>🛡️ License:</h2>

This project is licensed under the MIT License

## Like our work?💖

## Project Support 🚗 Welcome to the Automotive Forum Web App! If you have any questions encounter issues or simply want to chat about all things automotive we're here to help! Feel free to reach out by creating an issue in the \[Issues\](https://github.com/Web-AutomotiveForumSystem-A54/AutomotiveForumSystem/issues) section. Let's build a vibrant community together! Happy coding! 🏎️ The Automotive Forum Team

__________________________________________________________________________________________


To access SwaggerUI documentation, launch the program on your local machine and visit:
http://localhost:5000/swagger/index.html

Swagger raw JSON file: 
http://localhost:5000/swagger/v1/swagger.json

__________________________________________________________________________________________

How to use the AutomotiveForumSystemAPI:

## API Endpoints

### Auth

- **POST /api/auth/login**
  - Description: Log in and obtain an authentication token.
  - Parameters: 
    - `credentials` (Header, username and password split by :, no spaces): JWT token for authentication.
  - Responses:
    - `200 OK`: Successful login.

#### Example Request

```http
POST /api/auth/login
credentials: username:password
```
__________________________________________________________________________________________

### Categories

- **GET /api/categories**
  - Description: Get a list of categories.
  - Responses:
    - `200 OK`: Successful operation.

- **GET /api/categories/{id}**
  - Description: Get a category by its ID.
  - Parameters:
    - `id` (Path): The ID of the category to retrieve.
    - `Authorization` (Header): JWT token for authorization. Admin rights required.
  - Responses:
    - `200 OK`: Successful operation. Returns the requested category.
      - Response Body:
        - `CategoryDTO`: The detailed information about the category.
    - `404 Not Found`: The specified category ID was not found.
      - Response Body:
        - `string`: Error message describing the issue.

__________________________________________________________________________________________

- **POST /api/categories**
  - Description: Create a new category.
  - Parameters:
    - `Authorization` (Header): JWT token for authorization. Admin rights required.
  - Request Body: CategoryDTO
    - `Name` - required string in the DTO.
  - Responses:
    - `200 OK`: Successful operation. Returns the newly created category.
      - Response Body:
        - `CategoryDTO`: The detailed information about the created category.
    - `400 Bad Request`: A category with the same name already exists.
      - Response Body:
        - `string`: Error message describing the issue.

#### Example Request

```http
POST /api/categories
Authorization: Bearer <your_token_here>

{
  "Name": "New Category"
}
```
__________________________________________________________________________________________

- **PUT /api/categories/{id}**
  - Description: Update an existing category by ID.
  - Parameters:
    - `id` (Path): The ID of the category to update.
    - `Authorization` (Header): JWT token for authorization. Admin rights required.
  - Request Body: CategoryDTO
    - `Name` - required string in the DTO.
  - Responses:
    - `200 OK`: Successful operation. Returns the updated category.
      - Response Body:
        - `CategoryDTO`: The detailed information about the updated category.
    - `400 Bad Request`: A category with the same name already exists.
      - Response Body:
        - `string`: Error message describing the issue.
    - `404 Not Found`: The specified category ID was not found.
      - Response Body:
        - `string`: Error message describing the issue.

#### Example Request

```http
PUT /api/categories
Authorization: Bearer <your_token_here>

{
  "Name": "New Category Name"
}
```
__________________________________________________________________________________________

- **DELETE /api/categories/{id}**
  - Description: Delete an existing category by ID.
  - Parameters:
    - `id` (Path): The ID of the category to delete.
    - `Authorization` (Header): JWT token for authorization. Admin rights required.
  - Responses:
    - `200 OK`: Successful operation. Returns a confirmation message.
      - Response Body:
        - `string`: Confirmation message.
    - `404 Not Found`: The specified category ID was not found.
      - Response Body:
        - `string`: Error message describing the issue.

__________________________________________________________________________________________

### Posts

- **GET /api/posts**
  - Description: Retrieve a list of posts based on specified query parameters. If there are no query parameters, retrieves a list, containing all posts.
  - Parameters:
    - `Category` (Query): Filter posts by category name.
      - Type: `string`
    - `Title` (Query): Filter posts by title.
      - Type: `string`
  - Responses:
    - `200 OK`: Successful operation. Returns a list of posts.
      - Response Body:
        - Array of PostResponseDTO.
    - `404 Not Found`: No posts found based on the specified criteria.
      - Response Body:
        - `string`: Error message describing the issue.

__________________________________________________________________________________________

- **GET /api/posts/users/{id}**
  - Description: Retrieve a list of posts created by a specific user based on specified query parameters.
  - Parameters:
    - `id` (Path): User ID for whom to retrieve posts.
      - Type: `integer`
    - `Category` (Query): Filter posts by category name.
      - Type: `string`
    - `Title` (Query): Filter posts by title.
      - Type: `string`
  - Responses:
    - `200 OK`: Successful operation. Returns a list of posts.
      - Response Body:
        - Array of PostResponseDTO.
    - `404 Not Found`: No posts found for the specified user or criteria.
      - Response Body:
        - `string`: Error message describing the issue.

#### Example Request

```http
GET /api/posts/users/123?Category=Tuning&Title=toyota-supra-mk4-tuning-and-restoration
```
__________________________________________________________________________________________

- **GET /api/posts/{id}**
  - Description: Retrieve details of a specific post by its ID.
  - Parameters:
    - `id` (Path): Post ID for which to retrieve details.
      - Type: `integer`
  - Responses:
    - `200 OK`: Successful operation. Returns the details of the specified post.
      - Response Body:
        - PostResponseDTO.
    - `404 Not Found`: No post found for the specified ID.
      - Response Body:
        - `string`: Error message describing the issue.

#### Example Request

```http
GET /api/posts/123?Category=Tuning&Title=toyota-supra-mk4-tuning-and-restoration
```
__________________________________________________________________________________________

- **POST /api/posts**
  - Description: Create a new post.
  - Authorization: Requires a valid JWT token in the Authorization header.
  - Request Body: PostCreateDTO.
  - Responses:
    - `200 OK`: Successful operation. Returns details of the newly created post.
      - Response Body:
        - PostResponseDTO.
    - `400 Bad Request`: Invalid request. Check the request body or authorization token.
      - Response Body:
        - `string`: Error message describing the issue.

#### Example Request

```http
POST /api/posts
Authorization: Bearer <your_jwt_token_here>

{
  "categoryID": 1,
  "title": "Sample Title",
  "content": "This is a sample post content."
}
```
__________________________________________________________________________________________

### Posts

- **PUT /api/posts/{id}**
  - Description: Update an existing post by ID.
  - Authorization: Requires a valid JWT token in the Authorization header.
  - Parameters:
    - `id` (Path): ID of the post to be updated.
      - Type: `integer`
  - Request Body: PostCreateDTO.
  - Responses:
    - `200 OK`: Successful operation. Returns details of the updated post.
      - Response Body:
        - PostResponseDTO.
    - `400 Bad Request`: Invalid request. Check the request body or authorization token.
      - Response Body:
        - `string`: Error message describing the issue.
    - `404 Not Found`: No post found for the specified ID.
      - Response Body:
        - `string`: Error message describing the issue.

#### Example Request

```http
PUT /api/posts/123
Authorization: Bearer <your_jwt_token_here>

{
  "categoryID": 2,
  "title": "Updated Post",
  "content": "This is the updated content of the post."
}
```
__________________________________________________________________________________________

- **DELETE /api/posts/{id}**
  - Description: Delete an existing post by ID.
  - Authorization: Requires a valid JWT token in the Authorization header.
  - Parameters:
    - `id` (Path): ID of the post to be deleted.
      - Type: `integer`
  - Responses:
    - `200 OK`: Successful operation. Returns a success message.
      - Response Body:
        - `string`: "Post deleted successfully!"
    - `400 Bad Request`: Invalid request. Check the authorization token.
      - Response Body:
        - `string`: Error message describing the issue.
    - `404 Not Found`: No post found for the specified ID.
      - Response Body:
        - `string`: Error message describing the issue.

#### Example Request

```http
DELETE /api/posts/123
Authorization: Bearer <your_jwt_token_here>
```

__________________________________________________________________________________________
