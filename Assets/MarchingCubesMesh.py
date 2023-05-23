import numpy as np
from skimage import measure

class MarchingCubesMesh:
    def __init__(self, function:str, l, u, resolution):
        range = [l, u]
        self.function = function
        self.x_range = range
        self.y_range = range
        self.z_range = range
        self.resolution = resolution

        self.x = np.linspace(self.x_range[0], self.x_range[1], self.resolution)
        self.y = np.linspace(self.y_range[0], self.y_range[1], self.resolution)
        self.z = np.linspace(self.z_range[0], self.z_range[1], self.resolution)

        self.xv, self.yv, self.zv = np.meshgrid(self.x, self.y, self.z, indexing='ij')

        self.f = self.get_function(self.function)

        self.vertices = []
        self.triangles = []

        self.generate_mesh()

    def generate_mesh(self):
            space_between_vertices = 1 / (self.resolution - 1)
            print(space_between_vertices)
            accuracy = space_between_vertices

            # Evaluate the scalar field
            values = self.f(self.xv, self.yv, self.zv)

            # Generate the isosurface using the marching cubes algorithm
            verts, faces, _, _ = measure.marching_cubes(values, level=0)

            # Scale and shift the vertices to the desired range
            verts = verts * space_between_vertices
            verts[:, 0] += self.x_range[0]
            verts[:, 1] += self.y_range[0]
            verts[:, 2] += self.z_range[0]

            # Add the vertices and faces to the mesh data structure
            self.vertices = verts.tolist()
            self.triangles = faces.tolist()

    def get_index(self, i, j, k):
        return i * self.resolution * self.resolution + j * self.resolution + k

    def get_vertices(self):
        return self.vertices

    def get_triangles(self):
        return self.triangles

    def get_mesh(self):
        return self.vertices, self.triangles

    def get_function(self, functionstr):
        def f(x, y, z):
            return eval(functionstr)
        return f
