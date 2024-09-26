# Script to run generate input data for DiamondWorld
# Version 2.0
# Date:  26/09/2024
# Author: Joern Alexander Quent
# /* 
# ----------------------------- Note on this script ---------------------------
# */
#
# /* 
# ----------------------------- Libraries, settings and functions ---------------------------
# */
######################################################
setwd("E:/research_projects/OLM_project/experiment/unityProjects/DiamondWorld/nonUnity_folder")
######################################################

# Libs
library(plyr)
library(ggplot2)

# Seed
set.seed(20240926)

# /* 
# ----------------------------- Params ---------------------------
# */
# Spawn limits
x_min <- -43
x_max <- 43
z_min <- -43
z_max <- 43
minDist <- 3

# Number of objects
numDiamonds <- 20
numTraps <- 50

# Reactivation range
time_min <- 10
time_max <- 30

# /* 
# ----------------------------- Draw random locations ---------------------------
# */
# Draw respanwn timings
diamondTimings <- round(runif(numDiamonds, min = time_min, max = time_max), 1)
trapTimings <- round(runif(numTraps, min = time_min, max = time_max), 1)

# Function to draw random position
draw_position <- function(x_min, x_max, z_min, z_max){
  x <- round(runif(1, min = x_min, max = x_max), 1)
  z <- round(runif(1, min = z_min, max = z_max), 1)
  return(c(x, z))
}

# Function to scatter diamonds and traps with minimum distance
scatter_objects <- function(numDiamonds, numTraps, x_min, x_max, z_min, z_max, minDist) {
  # Create a list of objects to create
  objectList <- c(rep("diamond", numDiamonds), rep("trap", numTraps))
  objectList <- sample(objectList)
  
  # Create empty variables to store the position
  positions <- matrix(NA, nrow = 0, ncol = 2)
  
  # New line
  cat("\n")
  
  # Loop through the object list
  for(i in 1:length(objectList)){
    if(i == 1){
      # Draw a random position
      positions <- rbind(positions, draw_position(x_min, x_max, z_min, z_max))
    } else {
      # Draw a random position
      rand_pos <- draw_position(x_min, x_max, z_min, z_max)
      while_index <- 1
      
      # Check if this position's minimum distance is above minDist 
      # if not keep on drawing new position
      while(min(dist(rbind(positions, rand_pos))) < minDist){
        rand_pos <- draw_position(x_min, x_max, z_min, z_max)
        cat("\r Placing object", i, "Try", while_index)
        while_index <- while_index + 1
        if(i == 6){
          return(list(objects = objectList, positions = positions))
        }
      }
      
      # Add to matrix
      positions <- rbind(positions, rand_pos)
    }
  }
  
  return(list(objects = objectList, positions = positions))
}

# Scatter objects
object_locations <- scatter_objects(numDiamonds, numTraps, x_min, x_max, z_min, z_max, minDist)

# Split variables
diamond_x <- object_locations$positions[object_locations$objects == "diamond", 1]
diamond_z <- object_locations$positions[object_locations$objects == "diamond", 2]
trap_x <- object_locations$positions[object_locations$objects == "trap", 1]
trap_z <- object_locations$positions[object_locations$objects == "trap", 2]

# /* 
# ----------------------------- Visualisation ---------------------------
# */
# Add to one data frame
df <- data.frame(object = c(rep('diamond', numDiamonds), rep('trap', numTraps)),
                 x = c(diamond_x, trap_x),
                 z = c(diamond_z, trap_z),
                 timer = c(diamondTimings, trapTimings))

vis <- ggplot(data = df, aes(x = x, y = z, colour = object, size = timer)) + 
  geom_point() +
  labs(title = 'Diamond & trap distribution', x = "", y = "") +
  coord_equal(xlim = c(-45, 45), ylim = c(-45, 45), expand = FALSE) +
  theme_void() + 
  theme(axis.text.y   = element_text(size=14),
        axis.text.x   = element_text(size=14),
        axis.title.y  = element_text(size=14),
        axis.title.x  = element_text(size=14),
        panel.background = element_blank(),
        panel.grid.major = element_blank(), 
        panel.grid.minor = element_blank(),
        axis.line = element_line(colour = "black"),
        panel.border = element_rect(colour = "black", fill = NA, size = 5))

vis

ggsave(vis, filename= "objectDistribution.png", width = 3000, height = 3000, dpi = 300, units = "px")


# /* 
# ----------------------------- Create output ---------------------------
# */

paste0("[", paste0(diamond_x, collapse = ", "), "]")
paste0("[", paste0(diamond_z, collapse = ", "), "]")
paste0("[", paste0(diamondTimings, collapse = ", "), "]")
paste0("[", paste0(trap_x, collapse = ", "), "]")
paste0("[", paste0(trap_z, collapse = ", "), "]")
paste0("[", paste0(trapTimings, collapse = ", "), "]")