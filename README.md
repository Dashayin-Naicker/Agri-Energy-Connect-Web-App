# Agri‑Energy Connect Platform — User Manual

## 📺 Video Overview
**Website Walkthrough:** https://youtu.be/F4h0XNHtqAY

---

# 1. Before Running the Application

## ✅ Setup Steps
1. Download the ZIP file and extract it.
2. Open **Tools → Package Manager Console (PMC)** in Visual Studio.
3. Run migrations in the PMC:
   - `add-migration "MigrationName"`
   - `update-database`
4. The database name is **AgriEnergyConnectDb**, located on **Local SQL** in Visual Studio.
5. To view the database:
   - Go to **View → SQL Server Object Explorer**

---

# 2. After Running Migrations

# 2.1 Login & Registration Overview

## ✅ Launching the Application
- Run the application (**Ctrl + F5**).
- The Home Page will load with two navigation tabs:
  - **Home**
  - **Account** (dropdown)

## ✅ Account Dropdown
Hover over **Account** to access:
- **Register**
- **Login**

## ✅ Access Control
- Users **must** log in or register before accessing any functionality.
- Two roles exist:
  - **Farmer**
  - **Employee**

## ✅ Role Restrictions
- New users can **only register as Farmers** for security reasons.
- Employees **cannot self‑register**; only an existing employee can create another employee account.
- Employees can also register Farmers.

## ✅ Validation
- All Login/Registration pages include validation to guide the user.

---

# 3. Farmer Experience

## 3.1 Registering / Logging In as a Farmer
- After registering, the user is **automatically logged in**.
- Existing users must log in before accessing features.
- After login, the user is redirected to the **Farmer Dashboard**.

## 3.2 Farmer Dashboard
- A navigation panel appears on the left.
- Farmers can:
  - **Add a product** to the database.
  - **View all their products**.

---

# 4. Employee Experience

## 4.1 Logging In as an Employee
- Employees must log in before accessing features.
- Employees **cannot self‑register**; another registered employee must create their account.
- After login, the user is redirected to the **Employee Dashboard**.

## 4.2 Employee Dashboard
- A navigation panel appears on the left.
- Employees can:
  - **Register a new Farmer**
  - **Register another Employee**
  - Choose the role (Farmer or Employee) during registration
  - After registering a Farmer, they are redirected to **View Farmers** to see all farmers

## 4.3 Managing Farmer Products
Employees (with administrative rights) can:
- View **all farmer products**
- **Delete** any product

## 4.4 Viewing a Specific Farmer
Employees can:
- Select a specific farmer
- View all products associated with that farmer
- Filter products by:
  - **Product Category**
  - **Production Date**

---

# 5. Shared Features (Farmers & Employees)

## ✅ Logout
Both roles can log out from:
- The **Home Page**
- Their respective **Dashboards**

---

# 6. Validation & Security

- Validation is implemented throughout the website to prevent crashes.
- Users attempting to access unauthorized pages (e.g., a Farmer trying to access the Employee Dashboard) will be **redirected to the Home Page**.

---

# 7. UI Design

- The frontend is built using **Bootstrap templates**.
- The interface is responsive and works on **mobile devices**.
