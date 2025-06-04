export interface Project {
  id: string;
  name: string;
  region: string;
  project: string;
  status: string;
  description?: string;
  startDate?: Date;
  endDate?: Date;
  budget?: number;
  assignedUserId?: number;
  createdAt: Date;
  updatedAt: Date;
}

export interface CreateProjectRequest {
  name?: string;
  region: string;
  project: string;
  status: string;
  description?: string;
  startDate?: Date;
  endDate?: Date;
  budget?: number;
  assignedUserId?: number;
}

export interface UpdateProjectRequest {
  id: string;
  name?: string;
  region: string;
  project: string;
  status: string;
  description?: string;
  startDate?: Date;
  endDate?: Date;
  budget?: number;
  assignedUserId?: number;
}

export interface ProjectSite {
  id: string;
  projectId: string;
  name: string;
  location: string;
  coordinates?: {
    latitude: number;
    longitude: number;
  };
  description?: string;
  createdAt: Date;
  updatedAt: Date;
} 