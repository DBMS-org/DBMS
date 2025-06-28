export interface Project {
  id: number;
  name: string;
  region: string;
  status: string;
  description?: string;
  startDate?: Date;
  endDate?: Date;
  assignedUserId?: number;
  createdAt: Date;
  updatedAt: Date;
  isPatternApproved?: boolean;
  isSimulationConfirmed?: boolean;
}

export interface CreateProjectRequest {
  name: string;
  region: string;
  status: string;
  description?: string;
  startDate?: Date;
  endDate?: Date;
  assignedUserId?: number;
}

export interface UpdateProjectRequest {
  id: number;
  name: string;
  region: string;
  status: string;
  description?: string;
  startDate?: Date;
  endDate?: Date;
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
  isPatternApproved?: boolean;
  isSimulationConfirmed?: boolean;
} 