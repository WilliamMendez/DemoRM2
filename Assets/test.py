class ClassTest:
    def __init__(self):
        print("test")

    def test2(self):
        print("test")
        return 1

    def testMesh(self):
        points = []
        triangles = []
        points.append((0, 0, 0))
        points.append((1, 0, 0))
        points.append((0, 1, 0))
        points.append((0, 0, 1))
        points.append((1, 1, 0))
        points.append((1, 0, 1))
        points.append((0, 1, 1))
        points.append((1, 1, 1))
        triangles.append((0, 1, 2))
        triangles.append((1, 2, 4))
        triangles.append((1, 4, 5))
        triangles.append((1, 5, 3))
        triangles.append((1, 3, 0))
        triangles.append((0, 3, 6))
        triangles.append((0, 6, 2))
        triangles.append((2, 6, 7))
        triangles.append((2, 7, 4))
        triangles.append((4, 7, 5))
        triangles.append((5, 7, 3))
        triangles.append((3, 7, 6))


        return points, triangles

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

