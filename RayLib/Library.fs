namespace RayLib

module Raycasting =

    type RayHit = {
        Distance : float32
        Shade : byte
        Tile : int
    }

    let castRay (px: float32) (py: float32) (angle: float32) (map: int[,]) =

        let rayDirX = cos angle
        let rayDirY = sin angle

        let mapX = int px
        let mapY = int py

        let deltaDistX =
            if rayDirX = 0.0f then System.Single.PositiveInfinity
            else abs (1.0f / rayDirX)

        let deltaDistY =
            if rayDirY = 0.0f then System.Single.PositiveInfinity
            else abs (1.0f / rayDirY)

        let stepX, sideDistX =
            if rayDirX < 0.0f then
                -1, (px - float32 mapX) * deltaDistX
            else
                1, (float32 (mapX + 1) - px) * deltaDistX

        let stepY, sideDistY =
            if rayDirY < 0.0f then
                -1, (py - float32 mapY) * deltaDistY
            else
                1, (float32 (mapY + 1) - py) * deltaDistY

        // DDA
        let rec loop (mx, my, sdx, sdy) =
            if sdx < sdy then
                let next = sdx + deltaDistX
                let mx2 = mx + stepX
                if map.[my, mx2] <> 0 then
                    mx2, my, 0, next, sdy
                else
                    loop (mx2, my, next, sdy)
            else
                let next = sdy + deltaDistY
                let my2 = my + stepY
                if map.[my2, mx] <> 0 then
                    mx, my2, 1, sdx, next
                else
                    loop (mx, my2, sdx, next)

        let hitX, hitY, side, finalSideDistX, finalSideDistY =
            loop (mapX, mapY, sideDistX, sideDistY)

        let perpDist =
            if side = 0 then
                (float32 hitX - px + float32 (1 - stepX) / 2.0f) / rayDirX
            else
                (float32 hitY - py + float32 (1 - stepY) / 2.0f) / rayDirY

        let dist = abs perpDist

        let baseBrightness = 255.0f / (dist * 1.5f)

        let brightness =
            if side = 1 then baseBrightness * 0.7f
            else baseBrightness

        let shade = byte (min 255.0f (max 20.0f brightness))

        { Distance = dist
          Shade = shade
          Tile = map.[hitY, hitX] }
