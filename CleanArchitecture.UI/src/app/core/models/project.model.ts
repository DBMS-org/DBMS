export interface Project {
  id: number;
  name: string;
  region: string;
  projectType: string;
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
  projectType: string;
  status: string;
  description?: string;
  startDate?: Date;
  endDate?: Date;
  budget?: number;
  assignedUserId?: number;
}

export interface UpdateProjectRequest {
  id: number;
  name?: string;
  region: string;
  projectType: string;
  status: string;
  description?: string;
  startDate?: Date;
  endDate?: Date;
  budget?: number;
  assignedUserId?: number;
}

export interface ProjectSite {
  id: number;
  projectId: number;
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