export const ROLES = {
  ADMIN: 'admin',
  BLASTING_ENGINEER: 'blastingengineer',
  OPERATOR: 'operator',
  MECHANICAL_ENGINEER: 'mechanicalengineer',
  MACHINE_MANAGER: 'machinemanager'
} as const;

export type Role = typeof ROLES[keyof typeof ROLES];

export const USER_ROLES = {
  ADMIN: 'Admin',
  BLASTING_ENGINEER: 'BlastingEngineer',
  MECHANICAL_ENGINEER: 'MechanicalEngineer',
  MACHINE_MANAGER: 'MachineManager',
  OPERATOR: 'Operator'
} as const;

export const USER_ROLES_ARRAY = [
  USER_ROLES.ADMIN,
  USER_ROLES.BLASTING_ENGINEER,
  USER_ROLES.MECHANICAL_ENGINEER,
  USER_ROLES.MACHINE_MANAGER,
  USER_ROLES.OPERATOR
];

export type UserRole = typeof USER_ROLES[keyof typeof USER_ROLES]; 