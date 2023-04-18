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

export function add_to_scene(tree) {
    scene.add(tree);
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

export function create_visual(verts, inds, expected_block_count) {
    const geometry = new THREE.BufferGeometry();

    geometry.setIndex(new THREE.Uint32BufferAttribute(inds, 1));
    geometry.setAttribute('position', new THREE.Float32BufferAttribute(verts, 3));
    geometry.computeVertexNormals();

    const material = new THREE.MeshStandardMaterial({ color: 0xCCCCCC });

    const mesh = new THREE.Mesh(geometry, material);
    mesh.receiveShadow = true;
    mesh.castShadow = true;

    return mesh;
}

export function instantiate(tree, x, y, z, block_size_x, block_size_z, dir) {
    let clone = tree.clone();
    clone.visible = true;

    clone.position.set(x, y, z);

    clone.rotateY(-dir * Math.PI / 2);

    if (dir === 1) {
        clone.translateZ(-block_size_z);
    }
    else if (dir === 2) {
        clone.translateX(-block_size_x);
        clone.translateZ(-block_size_z);
    }
    else if (dir === 3) {
        clone.translateX(-block_size_x);
    }

    solid_instances.push(clone);
    scene.add(clone);
}