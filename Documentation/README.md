# Documentation

This folder contains project documentation.

## Contents
3.3	Functional Requirements   

3.3.1 Login & Role-Based Access 

The system shall display a login page with input fields for Username and Password. 

The system shall authenticate the credentials of the user against the stored user data. 

The system shall lock a user account after 5 consecutive failed login attempts. Upon account lock, the system shall display the message “Account locked – please reset your password”. 

The system shall inform the user trying to log in if their account is currently locked.  

The system shall provide a "Forgot Password" feature accessible from the login page. 

The "Forgot Password" feature shall send a time-limited password reset code to the user's registered email address. 

The password reset code shall expire after 10 minutes. 

The system shall implement Role-Based Access Control. Access to specific modules, features, and data shall be restricted based on the user’s assigned role. 

Upon successful login, the system shall display a role specific dashboard. 

The system shall automatically log out a user after 30 minutes of inactivity. 

Upon timeout due to inactivity, the system shall prompt the user for re-login to continue their session. 

3.3.2 User Management 

The system shall provide a User Management interface accessible only to the admin after authentication. 

The admin shall be able to create new user accounts via the User Management interface. The following fields shall be mandatory for user creation, Email, temporary password, First Name, Last Name, Home-Country Phone number, Resident-Country Phone number, Country of Origin, Address, and Role. 

The system shall reject the creation of a user account if the provided Email address is already registered. 

Upon successful user account creation, the system shall send an email notification to the newly created user. The email notification shall contain a temporary password that must be changed upon the first login.  

The admin shall be able to deactivate existing user accounts. 

The admin shall be able to delete existing user accounts. 

The system shall send email notifications to a user for any changes made to their account by admin. 

The User Management interface shall display a searchable list of all users. The list shall include Email, First Name, Last Name, Role, Status (Active or Deactivated), Creation Date excluding user password. 

The list shall be filterable by role, status and creation date. 

3.3.3 Project Management 

The system shall provide a Project Management interface accessible to general manager(admin). 

General Manager (Admin) shall be able to create new projects. 

The system shall require a unique Project ID, Project Name, Region, and Area of Operation. 

The system shall prompt to reenter details for the creation of a project upon entering if the provided Project ID or Project Name already exists. 

The system shall require entering Task description under each project. 

The system shall support uploading PDF and image files as attachments to a project. 

General Manager (Admin) shall be able to edit the details of existing projects. 

General Manager (Admin) shall be able to update the status of a project. Available project statuses shall include Pending, In Progress, and Completed. 

The Project Management interface shall display a list of projects filterable by Region and Status. 

The system shall allow assignment of team members and inventory items (Machines, Accessories) to projects. 

The system shall allow general manager to assign operator to machines. 

The system shall notify assigned users when they are added to a project. 

The system shall display a Project Dashboard showing Total, Pending, In Progress, and Completed Projects. 

The system shall allow General Manager (Admin) to archive completed projects. 

3.3.4 Project Site Management 

The system shall display a list of all Projects and their associated Sites. 

Blasting Engineers shall be able to select an assigned Project and create a new Site under that project. Site creation shall require a unique Site id, Name and Site Address for the Project. 

The system shall prompt to reenter site details if a Site Name with the same name already exists within the selected Project. 

Blasting Engineer shall be able to edit the details of existing Sites. 

3.3.5 Machines Inventory & Assignment 

The system shall provide a Machine Inventory interface accessible to Machine Manager and General Manager. 

Machine Manager shall be able to add details for machines. Required details shall include machine name, model, Serial Number, Rig No, Plate No, Company, Chassis Details, and Manufacturing Year. 

The system shall allow Machine manager to edit or delete existing machines. 

The system shall display the real-time availability or assignment status of each machine. 

The Machine Inventory interface shall support filtering of records. Filtering options shall include Model, Type, Status (Available, Assigned, Under Maintenance), and Project assignment. 

The system shall maintain history of all machine assignments and maintenance events with timestamps. 

The system shall allow General Manager to submit a machine assignment request specifying Project Id, Machine type, quantity and optional detail or explanation. 

The system shall route all machine assignment requests to the machine manager. 

The system shall allow machine manager to review all requests and assign or reject machines request with reason. 

The system shall notify the assigned machine operator and requesting General Manager of the assignment via in app notification. 

3.3.6 Explosive Management 

The system shall allow the Explosive manager to add a new store by adding Store Name, Store Address, Store Manager’s Name and Contact Information, Store Type, Explosive Types Available and Storage Capacity. 

The system shall generate and assign a unique Store ID upon successful store creation. 

The system shall notify the designated Store Manager when the store is created. 

The system shall allow the Explosive manager to edit any store’s details (name, address, manager information). 

The system shall require confirmation before deleting a store. 

Upon deletion confirmation, the system shall remove the store and all its active inventory records. 

After deletion, the system shall notify the affected Store Manager. 

The system shall allow the Explosive manager to deactivate a store while retaining all historical data. 

The system shall allow the Explosive manager to view, search, and filter all stores by status, location, and Store Manager. 

The system shall allow a Store Manager to submit a request for explosive stock entries, specifying batch number, quantity and expiry date. 

The system shall allow a Store Manager to submit a request to explosive manager for addition of explosive to existing explosive stock entries. 

The system shall route all add/edit/delete requests from Store Managers to the Explosive manager for approval. 

The system shall allow the Explosive manager to approve or reject each stock-change request. 

If a request is rejected, the system shall notify the originating Store Manager with a reason. 

Upon approval, the system shall apply the stock changes immediately. 

The system shall prevent Store Managers from making any stock changes unless approved by the Explosive manager. 

The system shall allow blasting engineer to submit explosive usage requests. 

Each request shall include quantity, project name and id, and a justification. 

The system shall route all explosive requests to the respective store manager for review based on their location. 

The system shall allow the Store manager to approve or deny each request with reason. 

If denied, the system shall notify the requester with a justification. 

The system shall mark approved quantities as "allocated" in the selected store. 

The system shall notify the relevant Store Manager through in-app notification to prepare the explosives for dispatch. 

The system shall log each assignment action with request details, approving actor, assigned store, and timestamp. 

The system shall allow the Explosive manager to set and modify low-stock thresholds for each explosive type and equipments. 

The system shall monitor stock levels across all stores and shall trigger a low-stock alert when stock falls below the defined threshold. 

The system shall send in-app notifications to the Explosive manager for each low-stock alert. 

The system shall allow to generate store-wise inventory reports. 

The system shall support exporting all reports to PDF format. 

3.3.7 Machine Accessories 

The system shall provide an Accessories Inventory interface accessible to Machine manager. 

Machin manager shall be able to track accessory items. Tracked fields shall include Name, Type, and Quantity. 

The system shall generate alerts when the quantity of an accessory type falls below a configurable low-stock threshold. Alerts shall be delivered via in-app notifications. 

The Accessories Inventory interface shall support filtering and searching of records. 

The system shall allow editing of existing accessory records. 

The system shall record the usage of accessories per Project. 

3.3.8 Maintenance Management for Machines 

The system shall provide a Maintenance Management interface accessible to Mechanical engineer and Machine Managers. 

Machine manager shall be able to schedule machine for maintenance. 

Machine operator shall be able to log daily usage data for machines. 

Daily usage data shall include engine hours, idle hours, service hour. 

The system shall calculate the remaining service hours for each machine based on logged usage and configured service intervals. 

The system shall trigger alerts when a machine approaches its defined service threshold based on remaining service hours. 

The system shall display the operational status of each machine, on machine inventory interface. The status shall reflect real-time conditions (Running, Idle, Under Maintenance, Breakdown, Available). 

3.3.9 Drilling Pattern Design & Survey 

The system shall provide a web-based 2D editor for designing drilling patterns. The editor shall support interactive zoom and pan functionality. 

The system shall validate key drilling pattern parameters entered by the user. Parameters subject to validation shall include hole diameter, spacing, burden, stemming, and depth.  

Users shall be able to save designed drilling patterns as named templates. 

Users shall be able to manage saved drilling pattern templates. Template management shall include options for editing, duplication, and deletion. 

The system shall allow users to assign millisecond delay timings and define connectors and detonators between holes in the pattern. 

The system shall render a 3D visualization of the designed drilling pattern. The 3D visualization shall be interactive, supporting zoom, pan, and rotate controls to view holes length, angle and depth. The 3D visualization shall update to reflect changes made in the 2D editor. 

The system shall simulate the blast sequence based on assigned delay timings. The simulation shall visually show the firing order of the holes. 

The system shall allow the import of survey data. Supported import formats shall include Excel files. 

Imported data shall include columns for Hole ID, Coordinates, Elevation, Depth, Azimuth, Dip, Stemming, Charge Type, and Charge Mass. 

The system shall display imported survey data in a tabular format for review by the blasting engineer. 

Blasting engineer shall be able to save or reject imported survey data after review. 

The system shall be able to provide the blast design to the operator for drilling accordingly. 

The system shall automatically calculate the total and per-hole explosive requirements. 

Calculations shall be based on validated formulas and the imported survey data.  

Blasting Engineers shall be able to manually override the automatically calculated explosive requirements. 

The system shall provide a side-by-side display comparing the planned drilling pattern parameters and the actual imported survey data. 

The system shall automatically recalculate explosive requirements whenever relevant survey data or pattern parameters are changed or overridden. 

The system shall export a summary report for a given blast. 

The report shall include a summary of survey data, the planned pattern image, and the explosive calculations. The report shall be exportable in PDF format. 

3.3.10 Analytics & Reports 

The system shall generate blast reports. Reports shall include details such as Project Name, Site, Pattern Image, Survey Data Summary, Explosive Usage, and Volume Calculations. 

The system shall provide a preview interface for reports. 

Reports shall be exportable in pdf formats. 

3.3.11 User Profile Management 

The system shall provide a User Profile page accessible to each logged-in user. 

Users shall be able to view and edit their personal details on the User Profile page. Editable fields shall include: First Name, Last Name, Phone Numbers (Home-Country Phone, Resident-Country Phone), Country of Origin, and Address. 

The system shall require the user to enter their current password before allowing a password change. 

The system shall enforce a minimum password complexity requirement of 8 characters. Required character types shall include at least one uppercase letter, one lowercase letter, one numeric digit, and one special character. 

The system shall validate the format of updated Phone Numbers and Address fields upon saving profile changes. 

The system shall send an email notification to the user whenever their profile details are changed, including password updates. 

The User Profile page shall include a "Save Changes" button. 

The "Save Changes" button shall remain disabled until the user makes valid changes to editable fields. 