export interface User {
  id: number;
  name: string;
  email: string;
  role: string;
  status: string;
  area: string;
  region: string;
  country: string;
  omanPhone: string;
  countryPhone: string;
  createdAt: Date;
  updatedAt: Date;
}

export interface CreateUserRequest {
  name: string;
  email: string;
  role: string;
  area: string;
  region: string;
  country: string;
  omanPhone: string;
  countryPhone: string;
}

export interface UpdateUserRequest {
  id: number;
  name: string;
  email: string;
  role: string;
  status: string;
  area: string;
  region: string;
  country: string;
  omanPhone: string;
  countryPhone: string;
}

export interface DatabaseConnectionTest {
  message: string;
  database: string;
  userCount: number;
  timestamp: Date;
} 