let cam;
let cam_target;
let cam_spherical;
let isShiftKeyDown = false;

const moveSpeed = 0.002;
const rotateSpeed = 0.004;

export function getCam() {
    return cam;
}

export function create(distance) {
    cam = new THREE.PerspectiveCamera(85, (window.innerWidth - 10) / (window.innerHeight - 80), 0.1, 10000);
    cam.position.z = -distance;

    cam_target = new THREE.Vector3(0, 0, 0);
    cam_spherical = new THREE.Spherical().setFromVector3(cam.position.clone().sub(cam_target));
    cam_spherical.phi = 1;
    
    document.addEventListener('keydown', onKeyDown);
    document.addEventListener('keyup', onKeyUp);

    return cam;
}

export function animate() {
    // Convert spherical coordinates to cartesian coordinates
    const position = new THREE.Vector3().setFromSpherical(cam_spherical).add(cam_target);

    // Set camera position and look at target
    cam.position.copy(position);
    cam.lookAt(cam_target);
}

export function move(x, y, z) {
    cam_target.set(x, y, z);
}

export function dispose() {
    document.removeEventListener('keydown', onKeyDown);
    document.removeEventListener('keyup', onKeyUp);
}

function onKeyDown(event) {
    if (event.code == "ShiftLeft") {
        isShiftKeyDown = true;
    }
    switch (event.code) {
        case 'KeyA':
            cam_spherical.theta -= rotateSpeed;
            break;
        case 'KeyD':
            cam_spherical.theta += rotateSpeed;
            break;
        case 'KeyS':
            cam_spherical.phi += rotateSpeed;
            break;
        case 'KeyW':
            cam_spherical.phi -= rotateSpeed;
            break;
    }
}

function onKeyUp(event) {
    if (event.code == "ShiftLeft") {
        isShiftKeyDown = false;
    }
}

let mouseDown = false;

export function onMouseDown(event) {
    mouseDown = true;
    if (event.code == "ShiftLeft" && event.button === 0) {
        isShiftKeyDown = true;
    }
}

export function onMouseUp(event) {
    mouseDown = false;
    if (event.code == "ShiftLeft" && event.button === 0) {
        isShiftKeyDown = true;
    }
}

export function onMouseMove(event) {
    if (mouseDown) {
        if (event.buttons === 1) {
            if (isShiftKeyDown) {
                const vector = new THREE.Vector3(-event.movementX * moveSpeed * cam_spherical.radius, event.movementY * moveSpeed * cam_spherical.radius, 0);
                vector.applyQuaternion(cam.quaternion);
                cam_target.add(vector);
            }
            else {
                const vector = new THREE.Vector3(-event.movementX * moveSpeed * cam_spherical.radius, 0, -event.movementY * moveSpeed * cam_spherical.radius);

                const quaternion = new THREE.Quaternion();
                cam.getWorldQuaternion(quaternion);

                // Separate the Y component of the vector
                const y = vector.y;
                vector.setY(0);

                // Transform the X and Z components of the vector into the camera's local coordinate system
                vector.applyQuaternion(quaternion);

                // Combine the transformed X and Z components with the original Y component
                vector.setY(y);

                cam_target.add(vector);
            }
        }
        else if (event.buttons === 2) {
            cam_spherical.theta -= event.movementX * rotateSpeed;
            cam_spherical.phi -= event.movementY * rotateSpeed;
        }
    }
}

export function onMouseWheel(event) {
    // Prevent the page from scrolling
    event.preventDefault();

    // Update the radius based on the scroll wheel delta
    cam_spherical.radius += event.deltaY * 0.001 * cam_spherical.radius;

    // Ensure the radius stays within the desired range
    cam_spherical.radius = Math.min(Math.max(cam_spherical.radius, 1), 256);
}