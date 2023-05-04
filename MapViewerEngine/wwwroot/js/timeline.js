import anime from './anime.es.js';

export function create(animations) {
    var tl = anime.timeline({
        easing: 'linear',
        loop: true,
        autoplay: false
    });
    
    for (let i = 0; i < animations.length; i++) {
        tl.add(animations[i], 0);
    }

    return tl;
}

export function createAnimation(target, keyframes) {
    return {
        targets: target,
        easing: 'linear',
        keyframes: keyframes,
        autoplay: false
    }
}

export function createKeyframeVec(duration, x, y, z) {
    return { duration: duration, x: x, y: y, z: z };
}

export function createKeyframeQuat(duration, x, y, z, w) {
    return { duration: duration, x: x, y: y, z: z, w: w };
}

export function play(timeline) {
    timeline.play();
}

export function pause(timeline) {
    timeline.pause();
}