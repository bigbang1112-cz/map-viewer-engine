const solid_instances = [];

const material = new THREE.MeshStandardMaterial({ color: 0xCCCCCC });

export function create_tree() {
    return new THREE.Object3D();
}

export function hide_tree(tree) {
    tree.visible = false;
}

export function add_to_tree(parent, child) {
    parent.add(child);
}

export function wrap_tree(tree) {
    let new_tree = create_tree();
    new_tree.add(tree);
    return new_tree;
}

export function set_tree_pos(tree, x, y, z) {
    tree.position.set(x, y, z);
}

export function set_tree_rot(tree, xx, xy, xz, yx, yy, yz, zx, zy, zz) {
    tree.rotation.setFromRotationMatrix(new THREE.Matrix4().set(xx, xy, xz, 0, yx, yy, yz, 0, zx, zy, zz, 0, 0, 0, 0, 1));
}

export function create_lod() {
    return new THREE.LOD();
}

export function add_lod(lod_tree, level_tree, distance) {
    lod_tree.addLevel(level_tree, distance);
}

export function create_visual(vertData, indData, uvData, expected_block_count) {

    var verts = new Float32Array(vertData.length / 4);
    var vertDataView = new DataView(vertData.slice().buffer);
    for (var i = 0; i < vertData.length; i += 4) {
        verts[i / 4] = vertDataView.getFloat32(i, true);
    }

    var inds = new Int32Array(indData.length);
    indData.copyTo(inds);

    const geometry = new THREE.BufferGeometry();
    geometry.setIndex(new THREE.Uint32BufferAttribute(inds, 1));
    geometry.setAttribute('position', new THREE.Float32BufferAttribute(verts, 3));

    var vertCount = verts.length / 3;
    var uvSetCount = uvData.length / 8 / vertCount;
    var uvBufferCount = uvData.length / uvSetCount;

    for (var i = 0; i < uvSetCount; i++) {
        var offset = i * uvBufferCount;
        var uvDataView = new DataView(uvData.slice(offset, offset + uvBufferCount).buffer);
        var uvs = new Float32Array(uvBufferCount / 4);
        for (var i = 0; i < uvBufferCount; i += 4) {
            uvs[i / 4] = uvDataView.getFloat32(i, true);
        }

        geometry.setAttribute('uv' + (i + 1), new THREE.Float32BufferAttribute(uvs, 2));
    }

    geometry.computeVertexNormals();

    const mesh = new THREE.InstancedMesh(geometry, material, expected_block_count);
    mesh.receiveShadow = true;
    mesh.castShadow = true;

    return mesh;
}

export function instantiate(tree, placements, bSizeX, bSizeZ, ebSizeX, ebSizeY, ebSizeZ) {
    tree.updateMatrix();
    tree.updateMatrixWorld();
    
    for (let i = 0; i < tree.children.length; i++) {
        instantiate(tree.children[i], placements, bSizeX, bSizeZ, ebSizeX, ebSizeY, ebSizeZ);
    }

    if (!tree.isInstancedMesh) {
        return;
    }
    
    for (let i = 0; i < placements.length; i++) {
        var blockPlacement = placements[i];

        var x = (blockPlacement >> 24) & 0xFF;
        var y = (blockPlacement >> 16) & 0xFF;
        var z = (blockPlacement >> 8) & 0xFF;
        var direction = blockPlacement & 0x0F;

        var worldMatrix = new THREE.Matrix4().copy(tree.matrixWorld);

        var normalizedMatrix = new THREE.Matrix4().copy(worldMatrix).invert().multiply(tree.matrix);

        var placementMatrix = getBlockPlacementMatrix(
            x * ebSizeX,
            y * ebSizeY,
            z * ebSizeZ,
            direction,
            bSizeX * ebSizeX,
            bSizeZ * ebSizeZ);

        var preFinalMatrix = normalizedMatrix.multiply(placementMatrix).multiply(worldMatrix);

        tree.setMatrixAt(i, preFinalMatrix);
    }

    tree.instanceMatrix.needsUpdate = true;
}

function getBlockPlacementMatrix(x, y, z, dir, bSizeX, bSizeZ) {
    const matrix = new THREE.Matrix4();

    matrix.makeRotationY(-dir * Math.PI / 2);

    if (dir === 0) {
        matrix.setPosition(x, y, z);
    }
    else if (dir === 1) {
        matrix.setPosition(x + bSizeX, y, z);
    }
    else if (dir === 2) {
        matrix.setPosition(x+bSizeX, y, z+bSizeZ);
    }
    else if (dir === 3) {
        matrix.setPosition(x, y, z + bSizeZ);
    }

    return matrix;
}

export function setUserData(tree, treeName, shaderName) {
    tree.userData = {
        treeName: treeName,
        shaderName: shaderName
    };
}

export function addUserData(tree, name, isGround, variant, subVariant) {
    tree.userData.type = "BlockVariant";
    tree.userData.name = name;
    tree.userData.isGround = isGround;
    tree.userData.variant = variant;
    tree.userData.subVariant = subVariant;
}

export function setShader(tree, shader) {
    tree.material = shader;
}

export function disposeInstances() {
    for (let i = 0; i < solid_instances.length; i++) {
        dispose(solid_instances[i], true);
    }
    solid_instances.length = 0;
}

function dispose(node, recursive = false) {
    if (!node) {
        return;
    }

    if (recursive && node.children) {
        for (const child of node.children) {
            dispose(child, recursive);
        }
    }

    if (node.geometry) {
        node.geometry.dispose();
    }

    if (!node.material) {
        return;
    }

    const materials = node.material.length === undefined ? [node.material] : node.material

    for (const material of materials) {
        for (const key in material) {
            const value = material[key];

            if (value && typeof value === 'object' && 'minFilter' in value) {
                value.dispose();
            }
        }

        material && material.dispose();
    }
}